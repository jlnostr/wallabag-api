using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            client = client.WithTimeout(5);

            await client.RefreshAccessTokenAsync();

            client = client.WithTimeout(100000);
        }
    }
}
