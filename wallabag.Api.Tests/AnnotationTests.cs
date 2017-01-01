using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class AnnotationTests : TestBaseClass
    {
        private WallabagItem _sampleItem;
        private WallabagAnnotation _sampleAnnotation;
        public override async Task InitializeAsync()
        {
            _sampleItem = await Client.AddAsync(new Uri("http://www.zeit.de/2016/51/autorennen-berlin-mord-klage-urteil"));
            Assert.IsNotNull(_sampleItem);
            ItemsToDelete.Add(_sampleItem);

            await EnsureValidityOfSampleAnnotationAsync();
        }

        [TestMethod]
        public async Task AddAnnotation()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(2))
            {
                var newAnnotation = CreateSampleAnnotation("My new annotation");
                _sampleAnnotation = await Client.AddAnnotationAsync(_sampleItem, newAnnotation);

                Assert.IsNotNull(_sampleAnnotation);
                Assert.IsTrue(_sampleAnnotation.Id > 0);
            }
        }

        [TestMethod]
        public async Task GetAnnotations()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(2))
            {
                var annotations = await Client.GetAnnotationsAsync(_sampleItem);

                Assert.IsNotNull(annotations);
                Assert.IsTrue(annotations.ToList().Count > 0);
            }
        }

        [TestMethod]
        public async Task DeleteAnnotation()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(2))
            {
                await EnsureValidityOfSampleAnnotationAsync();

                bool result = await Client.DeleteAnnotationAsync(_sampleAnnotation);
                Assert.IsTrue(result);

                var annotations = await Client.GetAnnotationsAsync(_sampleItem);
                Assert.IsTrue(annotations.Contains(_sampleAnnotation) == false);
            }
        }

        [TestMethod]
        public async Task UpdateAnnotation()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(2))
            {
                await EnsureValidityOfSampleAnnotationAsync();

                _sampleAnnotation.Text = "This is my new text";

                var newAnnotation = await Client.UpdateAnnotationAsync(_sampleAnnotation, _sampleAnnotation);
                Assert.IsNotNull(newAnnotation);
                Assert.IsTrue(newAnnotation.Text == _sampleAnnotation.Text);
            }
        }

        [TestMethod]
        public async Task DeleteAnnotationWithInvalidIdFails()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(2))
            {
                bool result = await Client.DeleteAnnotationAsync(999999999);
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AddAnnotationWithNoAnnotationFails()
        {
            await Client.AddAnnotationAsync(_sampleItem, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task AddAnnotationWithZeroRangesFails()
        {
            await Client.AddAnnotationAsync(_sampleItem, new WallabagAnnotation(new List<WallabagAnnotationRange>(), "Sample text"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task AddAnnotationWithInvalidRangePropertiesFails()
        {
            await Client.AddAnnotationAsync(_sampleItem, new WallabagAnnotation(new WallabagAnnotationRange(), "Sample text"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task AddAnnotationWithEmptyStartAndEndFails()
        {
            await Client.AddAnnotationAsync(_sampleItem, new WallabagAnnotation(new WallabagAnnotationRange(string.Empty, 0, string.Empty, 0), "Sample text"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task AddAnnotationWithInvalidRangeStartOrEndFails()
        {
            await Client.AddAnnotationAsync(_sampleItem, new WallabagAnnotation(new WallabagAnnotationRange("/a[123]", 0, "a/[123]", 0), "Sample text"));
        }

        private async Task EnsureValidityOfSampleAnnotationAsync()
        {
            if ((_sampleAnnotation == null || _sampleAnnotation.Id == 0) && await Client.MinorIsGreaterOrEqualAsync(2))
            {
                _sampleAnnotation = await Client.AddAnnotationAsync(_sampleItem, CreateSampleAnnotation());
                Assert.IsNotNull(_sampleAnnotation);
            }
        }

        private WallabagAnnotation CreateSampleAnnotation(string text = "Sample annotation")
            => new WallabagAnnotation(new WallabagAnnotationRange("/p[0]", 0, "/p[1]", 0), text);
    }
}
