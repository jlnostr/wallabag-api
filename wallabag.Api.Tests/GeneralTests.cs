using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class GeneralTests : TestBaseClass
    {
        private Regex _versionNumberRegex = new Regex("2.\\d+.\\d+");

        [TestMethod]
        public async Task VersionNumberReturnsValidValue()
        {
            var version = await Client.GetVersionNumberAsync();
            Assert.IsTrue(_versionNumberRegex.Match(version).Success);
        }

        [TestMethod]
        public async Task VersionNumberReturnsWithoutCredentials()
        {
            var sampleClient = new WallabagClient(Client.InstanceUri, string.Empty, string.Empty);

            var version = await Client.GetVersionNumberAsync();
            Assert.IsTrue(_versionNumberRegex.Match(version).Success);
        }

        [TestMethod]
        public void InitializationFailsWithInvalidUri()
        {
            AssertExtensions.ThrowsExceptionAsync<UriFormatException>(() =>
            {
                var test = new WallabagClient(new Uri(""), clientId, clientSecret);
                return Task.CompletedTask;
            });
        }

        [TestMethod]
        public async Task VersionReturnsValidValue()
        {
            var version = await Client.GetVersionAsync();
            Assert.IsNotNull(version);
            Assert.IsTrue(version.Major == 2);
            Assert.IsTrue(version.Minor >= 0);
            Assert.IsTrue(version.Build >= 0);
        }
    }
}
