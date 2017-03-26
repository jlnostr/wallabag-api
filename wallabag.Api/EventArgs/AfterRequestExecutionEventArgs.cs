using System.Net.Http;

namespace wallabag.Api.EventArgs
{
    /// <summary>
    /// The arguments of the <see cref="WallabagClient.AfterRequestExecution" /> event.
    /// </summary>
    public class AfterRequestExecutionEventArgs : PreRequestExecutionEventArgs
    {
        public HttpResponseMessage Response { get; set; }

        public AfterRequestExecutionEventArgs(PreRequestExecutionEventArgs preEventArgs, HttpResponseMessage response)
        {
            Response = response;

            Parameters = preEventArgs.Parameters;
            RequestMethod = preEventArgs.RequestMethod;
            RequestUriSubString = preEventArgs.RequestUriSubString;
        }
    }
}
