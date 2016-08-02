using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Exceptions")]
        public async Task ExceptionIsThrownWhenTimeoutIsReached()
        {
            var previousTimeout = client.Timeout;
            client.Timeout = 5;
            
            await AssertExtensions.ThrowsExceptionAsync<TimeoutException>(async () => await client.RefreshAccessTokenAsync());

            client.Timeout = previousTimeout;
        }
    }
}
