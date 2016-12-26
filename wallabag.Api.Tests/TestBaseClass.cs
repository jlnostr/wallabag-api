using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallabag.Api.Models;
using static wallabag.Api.Tests.Credentials;

namespace wallabag.Api.Tests
{
    public abstract class TestBaseClass
    {
        public WallabagClient Client { get; set; }
        public List<WallabagItem> ItemsToDelete { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            ItemsToDelete = new List<WallabagItem>();

            Client = new WallabagClient(new Uri(wallabagUrl), clientId, clientSecret);
            Client.RequestTokenAsync(username, password).Wait();

            InitializeAsync().Wait();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupAsync().Wait();

            foreach (var item in ItemsToDelete)
                Client.DeleteAsync(item).Wait();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task CleanupAsync() => Task.CompletedTask;
    }
}
