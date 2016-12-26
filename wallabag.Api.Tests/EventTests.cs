using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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

        private void Client_AfterRequestExecution(object sender, System.Net.Http.HttpResponseMessage e)
        {
            Assert.IsFalse(e.IsSuccessStatusCode);
        }
    }
}
