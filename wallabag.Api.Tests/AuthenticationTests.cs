using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Authentication")]
        public async Task RefreshingTokenFailsIfThereIsNoRefreshToken()
        {
            var oldToken = client.RefreshToken;
            client.RefreshToken = string.Empty;
            await AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await client.RefreshAccessTokenAsync());
            client.RefreshToken = oldToken;
        }

        [TestMethod]
        [TestCategory("Authentication")]
        public async Task RequestTokenSetsAccessAndRefreshToken()
        {
            await client.RequestTokenAsync(username, password);
            Assert.IsTrue(!string.IsNullOrEmpty(client.AccessToken));
            Assert.IsTrue(!string.IsNullOrEmpty(client.RefreshToken));
        }

        [TestMethod]
        [TestCategory("Authentication")]
        public void RequestTokenFailsWithoutCredentials()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await client.RequestTokenAsync(string.Empty, string.Empty));
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await client.RequestTokenAsync("username", string.Empty));
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await client.RequestTokenAsync(string.Empty, "password"));
        }

        [TestMethod]
        [TestCategory("Authentication")]
        public async Task LastTokenRefreshDateTimeInPastRefreshesToken()
        {
            await client.RequestTokenAsync(username, password);

            var oldAccessToken = client.AccessToken;
            var oldRefreshToken = client.RefreshToken;

            client.LastTokenRefreshDateTime = DateTime.UtcNow - TimeSpan.FromHours(6);

            Assert.IsTrue(await client.RefreshAccessTokenAsync());
            Assert.AreNotEqual(oldAccessToken, client.AccessToken);
            Assert.AreNotEqual(oldRefreshToken, client.RefreshToken);
        }
    }
}
