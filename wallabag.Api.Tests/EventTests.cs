using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using wallabag.Api.EventArgs;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class EventTests : TestBaseClass
    {
        [TestMethod]
        public void CredentialsRefreshedEventIsFiredCorrect()
        {
            var oldAccessToken = Client.AccessToken;
            var oldRefreshToken = Client.RefreshToken;
            var oldRefreshDateTime = Client.LastTokenRefreshDateTime;

            Client.CredentialsRefreshed += (s, e) =>
            {
                Assert.IsNotNull(Client.AccessToken);
                Assert.IsNotNull(Client.RefreshToken);
                Assert.IsNotNull(Client.LastTokenRefreshDateTime);
                Assert.AreNotEqual(oldAccessToken, Client.AccessToken);
                Assert.AreNotEqual(oldRefreshToken, Client.RefreshToken);
                Assert.AreNotEqual(oldRefreshDateTime, Client.LastTokenRefreshDateTime);
            };
        }

        [TestMethod]
        public async Task PreRequestEventContainsCorrectEventArgs()
        {
            Client.PreRequestExecution += Client_PreRequestExecution;
            await Client.DeleteAsync(999999);
            Client.PreRequestExecution -= Client_PreRequestExecution;
        }

        private void Client_PreRequestExecution(object sender, PreRequestExecutionEventArgs e)
        {
            Assert.IsNotNull(e);
            Assert.IsNotNull(e.RequestMethod);
            Assert.IsNotNull(e.RequestUriSubString);
            Assert.IsTrue(e.RequestMethod == WallabagClient.HttpRequestMethod.Delete);
            StringAssert.Contains(e.RequestUriSubString, 999999.ToString());
        }

        [TestMethod]
        public async Task AfterRequestEventContainsCorrectEventArgs()
        {
            Client.AfterRequestExecution += Client_AfterRequestExecution;

            await Client.DeleteAsync(123456789);

            Client.AfterRequestExecution -= Client_AfterRequestExecution;
        }

        private void Client_AfterRequestExecution(object sender, AfterRequestExecutionEventArgs e) => Assert.IsFalse(e.Response.IsSuccessStatusCode);

        [TestMethod]
        public async Task AfterRequestEventIsFiredIfHttpRequestFails()
        {
            int eventCounter = 0;
            var oldUrl = Client.InstanceUri;
            Client.InstanceUri = new System.Uri("http://127.0.0.1/");

            void afterRequestExecution(object sender, AfterRequestExecutionEventArgs e)
            {
                Assert.IsNotNull(e);
                Assert.IsNull(e.Response);
                eventCounter++;
            };

            Client.AfterRequestExecution += afterRequestExecution;

            await Client.DeleteAsync(123456789);

            Client.AfterRequestExecution -= afterRequestExecution;

            Assert.IsTrue(eventCounter > 0);

            Client.InstanceUri = oldUrl;
        }

        [TestMethod]
        public async Task PreRequestEventIsFiredForAuthenticationRequests()
        {
            bool fired = false;
            string refreshToken = Client.RefreshToken;
            Client.RefreshToken = "12345";
            Client.LastTokenRefreshDateTime = Client.LastTokenRefreshDateTime.Subtract(TimeSpan.FromHours(2));

            Client.PreRequestExecution += (s, e) =>
            {
                if (e.RequestUriSubString.Contains("oauth"))
                    fired = true;
            };

            await Client.DeleteAsync(123456789);
            Assert.IsTrue(fired);

            Client.RefreshToken = refreshToken;
        }

        [TestMethod]
        public async Task AfterRequestEventIsFiredForAuthenticationRequests()
        {
            bool fired = false;
            string refreshToken = Client.RefreshToken;
            Client.RefreshToken = "12345";
            Client.LastTokenRefreshDateTime = Client.LastTokenRefreshDateTime.Subtract(TimeSpan.FromHours(2));

            Client.AfterRequestExecution += (s, e) =>
            {
                if (!fired)
                {
                    StringAssert.Contains(e.RequestUriSubString, "oauth");
                    fired = true;
                }
            };

            await Client.DeleteAsync(123456789);
            Assert.IsTrue(fired);

            Client.RefreshToken = refreshToken;
        }
    }
}
