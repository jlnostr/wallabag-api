using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class AuthenticationTests : TestBaseClass
    {
        [TestMethod]
        public async Task RefreshingTokenFailsIfThereIsNoRefreshToken()
        {
            var oldToken = Client.RefreshToken;
            Client.RefreshToken = string.Empty;
            await AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await Client.RefreshAccessTokenAsync());
            Client.RefreshToken = oldToken;
        }

        [TestMethod]
        public async Task RequestTokenSetsAccessAndRefreshToken()
        {
            await Client.RequestTokenAsync(username, password);
            Assert.IsTrue(!string.IsNullOrEmpty(Client.AccessToken));
            Assert.IsTrue(!string.IsNullOrEmpty(Client.RefreshToken));
        }

        [TestMethod]
        public void RequestTokenFailsWithoutCredentials()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await Client.RequestTokenAsync(string.Empty, string.Empty));
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await Client.RequestTokenAsync("username", string.Empty));
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => await Client.RequestTokenAsync(string.Empty, "password"));
        }

        [TestMethod]
        public async Task LastTokenRefreshDateTimeInPastRefreshesToken()
        {
            await Client.RequestTokenAsync(username, password);

            var oldAccessToken = Client.AccessToken;
            var oldRefreshToken = Client.RefreshToken;

            Client.LastTokenRefreshDateTime = DateTime.UtcNow - TimeSpan.FromHours(6);

            Assert.IsTrue(await Client.RefreshAccessTokenAsync());
            Assert.AreNotEqual(oldAccessToken, Client.AccessToken);
            Assert.AreNotEqual(oldRefreshToken, Client.RefreshToken);
        }
    }
}
