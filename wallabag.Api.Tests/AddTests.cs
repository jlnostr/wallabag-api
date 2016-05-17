using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Add")]
        public void AddArticleWithoutUriFails()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await client.AddAsync(null);
            });
        }

        [TestMethod]
        [TestCategory("Add")]
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
                var result = await client.AddAsync(new Uri(item.Key), new string[] { item.Value });
                StringAssert.Contains(result.Tags.ToCommaSeparatedString(), item.Value);
            }
        }

        [TestMethod]
        [TestCategory("Add")]
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
                var result = await client.AddAsync(new Uri(item.Key), title: item.Value);
                StringAssert.Equals(result.Title, item.Value);
            }
        }
    }
}
