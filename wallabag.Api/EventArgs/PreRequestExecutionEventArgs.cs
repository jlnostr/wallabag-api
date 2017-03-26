using System.Collections.Generic;

namespace wallabag.Api.EventArgs
{
    /// <summary>
    /// The arguments of the <see cref="WallabagClient.PreRequestExecution" /> event.
    /// </summary>
    public class PreRequestExecutionEventArgs
    {
        /// <summary>
        /// The substring that will attached to the <see cref="WallabagClient.InstanceUri"/> to perform a certain HTTP request.
        /// </summary>
        public string RequestUriSubString { get; set; }

        /// <summary>
        /// The type of the HTTP request.
        /// </summary>
        public WallabagClient.HttpRequestMethod RequestMethod { get; set; }

        /// <summary>
        /// Any parameters that are going to be submitted along with the request, e.g. the URL of a new item.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
    }

}
