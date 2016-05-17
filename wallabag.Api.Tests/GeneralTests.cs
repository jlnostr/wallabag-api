using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    [TestClass]
    public partial class GeneralTests
    {
        WallabagClient client;

        [TestInitialize]
        public void InitializeUnitTests()
        {
            client = new WallabagClient(new Uri(wallabagUrl), clientId, clientSecret);
            var task = client.RequestTokenAsync(username, password);
            task.Wait();
        }

        [TestMethod]
        [TestCategory("General")]
        public async Task VersionNumberReturnsValidValue()
        {
            var version = await client.GetVersionNumberAsync();
            Assert.IsTrue(version.Contains("2.0"));
        }

        [TestMethod]
        [TestCategory("General")]
        public void InitializationFailsWithInvalidUri()
        {
            AssertExtensions.ThrowsExceptionAsync<UriFormatException>(() =>
            {
                var test = new WallabagClient(new Uri(""), clientId, clientSecret);
                return Task.CompletedTask;
            });
        }
    }
}
