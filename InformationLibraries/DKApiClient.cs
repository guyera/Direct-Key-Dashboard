using System;
using System.Net.Http;

namespace InformationLibraries
{
    // HttpClient exclusively for requests to / from the DirectKey API.
    // Message authentication configured in DKApiMessageHandler
    public class DKApiClient : HttpClient {

        public DKApiClient(DKApiMessageHandler handler) : base(handler) {
            // The BaseAddress is prepended to all relative
            // URI paths (such as paths not starting with '/').
            const string ApiAddress = "https://dkintapi.keytest.net/api/ver6/";
            BaseAddress = new Uri(ApiAddress);
        }
    }
}