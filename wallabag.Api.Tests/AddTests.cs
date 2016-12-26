using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class AddTests : TestBaseClass
    {
        [TestMethod]
        public void AddArticleWithoutUriFails()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await Client.AddAsync(null);
            });
        }

        [TestMethod]
        public async Task AddArticleWithSampleTag()
        {
            Dictionary<string, string> sampleItems = new Dictionary<string, string>()
            {
                ["http://www.zeit.de/2016/18/udo-lindenberg-atlantic-hotel-rockstar"] = "lindenberg",
                ["https://www.youtube.com/watch?v=HMQkV5cTuoY"] = "böhmermann",
                ["https://www.youtube.com/watch?v=rHia2TDEUmc"] = "youtube"
            };

            foreach (var item in sampleItems)
            {             
                var result = await Client.AddAsync(new Uri(item.Key), new string[] { item.Value });
                Assert.IsNotNull(result);
                StringAssert.Contains(result.Tags.ToCommaSeparatedString(), item.Value);

                ItemsToDelete.Add(result);
            }
        }

        [TestMethod]
        public async Task AddArticleWithGivenTitle()
        {
            Dictionary<string, string> sampleItems = new Dictionary<string, string>()
            {
                ["http://www.faz.net/aktuell/wirtschaft/neue-mobilitaet/autonomes-fahren-im-test-bei-mercedes-bmw-und-audi-14237392.html"] = "Yay, autonomes Fahren :)",
                ["http://www.zeit.de/digital/internet/2016-05/bundesgerichtshof-wlan-internet-nutzung-gaeste-filesharing"] = "Störerhaftung und so.",
                ["http://www.zeit.de/digital/internet/2016-05/xkcd-randall-munroe-webcomic"] = "xkcd <3"
            };

            foreach (var item in sampleItems)
            {
                var result = await Client.AddAsync(new Uri(item.Key), title: item.Value);
                Assert.IsNotNull(result);
                StringAssert.Equals(result.Title, item.Value);

                ItemsToDelete.Add(result);
            }
        }
    }
}
