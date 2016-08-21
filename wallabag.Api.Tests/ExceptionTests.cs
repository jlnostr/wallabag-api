using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Exceptions")]
        [ExpectedException(typeof(TaskCanceledException))]
        public async Task ExceptionIsThrownWhenTimeoutIsReached()
        {
            this.client = client.WithTimeout(5);

            await client.RefreshAccessTokenAsync();

            this.client = client.WithTimeout(100000);
        }
    }
}
