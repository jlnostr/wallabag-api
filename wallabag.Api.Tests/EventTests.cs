using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Events")]
        public async Task CredentialsRefreshedEventIsFiredCorrect()
        {
            var oldAccessToken = client.AccessToken;
            var oldRefreshToken = client.RefreshToken;
            var oldRefreshDateTime = client.LastTokenRefreshDateTime;

            client.CredentialsRefreshed += (s, e) =>
            {
                Assert.AreNotEqual(oldAccessToken, client.AccessToken);
                Assert.AreNotEqual(oldRefreshToken, client.RefreshToken);
                Assert.AreNotEqual(oldRefreshDateTime, client.LastTokenRefreshDateTime);
            };
        }

        [TestMethod]
        [TestCategory("Events")]
        public async Task PreRequestEventContainsCorrectEventArgs()
        {
            client.PreRequestExecution += Client_PreRequestExecution;
            await client.DeleteAsync(999999);
            client.PreRequestExecution -= Client_PreRequestExecution;
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
        [TestCategory("Events")]
        public async Task AfterRequestEventContainsCorrectEventArgs()
        {
            client.AfterRequestExecution += Client_AfterRequestExecution;

            await client.DeleteAsync(123456789);

            client.AfterRequestExecution -= Client_AfterRequestExecution;
        }

        private void Client_AfterRequestExecution(object sender, System.Net.Http.HttpResponseMessage e)
        {
            Assert.IsFalse(e.IsSuccessStatusCode);
        }
    }
}
