using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    [TestClass]
    public partial class GeneralTests
    {
        WallabagClient client;
        private Regex _versionNumberRegex = new Regex("2.\\d+.\\d+");

        [TestInitialize]
        public void InitializeUnitTests()
        {
            client = new WallabagClient(new Uri(wallabagUrl), clientId, clientSecret);
            client.RequestTokenAsync(username, password).Wait();
        }

        [TestMethod]
        [TestCategory("General")]
        public async Task VersionNumberReturnsValidValue()
        {
            var version = await client.GetVersionNumberAsync();
            Assert.IsTrue(_versionNumberRegex.Match(version).Success);
        }

        [TestMethod]
        [TestCategory("General")]
        public async Task VersionNumberReturnsWithoutCredentials()
        {
            var sampleClient = new WallabagClient(client.InstanceUri, string.Empty, string.Empty);
                        
            var version = await client.GetVersionNumberAsync();
            Assert.IsTrue(_versionNumberRegex.Match(version).Success);
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
