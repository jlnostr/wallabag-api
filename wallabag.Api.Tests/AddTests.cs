using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Add")]
        public void AddArticleWithoutUriFails()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await client.AddAsync(null);
            });
        }

        [TestMethod]
        [TestCategory("Add")]
        [DataRow("http://www.zeit.de/politik/ausland/2016-04/oesterreich-kommentar-praesidentenwahl-triumph-rechtspopulisten", "politik")]
        [DataRow("http://www.nytimes.com/2016/04/25/opinion/europes-web-privacy-rules-bad-for-google-bad-for-everyone.html?ref=technology", "tech")]
        public async Task AddArticleWithSampleTag(string url, string tag)
        {
            var item = await client.AddAsync(new Uri(url), new string[] { tag });
            Assert.IsTrue(item.Tags.ToCommaSeparatedString().Contains(tag));
        }

        [TestMethod]
        [TestCategory("Add")]
        [DataRow("http://www.nytimes.com/2016/04/23/arts/music/prince-music-technology-distribution.html?ref=technology", "Prince was an musician")]
        [DataRow("http://www.nytimes.com/2016/02/25/technology/personaltech/tips-and-myths-about-extending-smartphone-battery-life.html?ref=technology", "Unit testing ftw!")]
        public async Task AddArticleWithGivenTitle(string url, string title)
        {
            var item = await client.AddAsync(new Uri(url), title: title);
            Assert.IsTrue(item.Title == title);
        }
    }
}
