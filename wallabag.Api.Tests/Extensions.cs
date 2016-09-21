using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallabag.Api.Tests
{
    // Kudos to Pedro Lamas: https://www.pedrolamas.com/2013/04/30/assert-throwsexception-the-async-way/
    public static class AssertExtensions
    {
        public static Task<T> ThrowsExceptionAsync<T>(Func<Task> action)
            where T : Exception
        {
            return ThrowsExceptionAsync<T>(action, string.Empty, null);
        }

        public static Task<T> ThrowsExceptionAsync<T>(Func<Task> action, string message)
            where T : Exception
        {
            return ThrowsExceptionAsync<T>(action, message, null);
        }

        public async static Task<T> ThrowsExceptionAsync<T>(Func<Task> action, string message, object[] parameters)
            where T : Exception
        {
            try { await action(); }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(T))
                    return ex as T;

                var objArray = new object[] {
                    "AssertExtensions.ThrowsExceptionAsync",
                    string.Format(CultureInfo.CurrentCulture,  message, typeof(T).Name, ex.GetType().Name, ex.Message, ex.StackTrace)
                };

                throw new AssertFailedException(UnitTestOutcome.Failed.ToString());
            }

            var objArray2 = new object[] {
                "AssertExtensions.ThrowsExceptionAsync",
                string.Format(CultureInfo.CurrentCulture, "No exception thrown.", message, typeof(T).Name)
            };

            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "Failed.", UnitTestOutcome.Failed, objArray2));
        }
    }

    public static class WallabagClientExtensions
    {
        public static WallabagClient WithTimeout(this WallabagClient client, int timeout)
        {
            return new WallabagClient(client.InstanceUri, client.ClientId, client.ClientSecret, timeout, client.FireHtmlExceptions)
            {
                AccessToken = client.AccessToken,
                LastTokenRefreshDateTime = client.LastTokenRefreshDateTime,
                RefreshToken = client.RefreshToken
            };
        }
        public static async Task<bool> VersionEqualsAsync(this WallabagClient client, string version)
        {
            var clientVersion = await client.GetVersionNumberAsync();
            return clientVersion.Contains(version);
        }
    }
}
