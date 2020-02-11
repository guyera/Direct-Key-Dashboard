using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace InformationLibraries {
    // DKApiAccess is an injected singleton wrapped around
    // a DKApiClient used to communicate with the API.
    // This is preferred over directly using the DKApiClient
    // as SendAsync should not be used as a public facing
    // function when a simple wrapper function can be used instead.
    public class DKApiAccess {
        private readonly DKApiClient Client;
        public DKApiAccess() {
            DKApiMessageHandler handler = new DKApiMessageHandler();
            Client = new DKApiClient(handler);
        }

        // Just for testing purposes
        public async Task<string> PullKeyDeviceActivity() {
            return await PullKeyDeviceActivity(DateTime.ParseExact("2019-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2019-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        public async Task<string> PullKeyDeviceActivity(DateTime entryDateStart, DateTime entryDateEnd) {
            string path = $"KeyDeviceActivity?entryDateStart={entryDateStart.Month}%2F{entryDateStart.Day}%2F{entryDateStart.Year}&entryDateEnd={entryDateEnd.Month}%2F{entryDateEnd.Day}%2F{entryDateEnd.Year}&takes=2000";
            var response = await Client.GetAsync(path);
        
            Console.WriteLine("Status: " + Enum.GetName(typeof(HttpStatusCode), response.StatusCode));
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PullNewest()
        {
            // NOTE: In order for the BaseAddress of the Client to work,
            // the BaseAddress must end with a '/' and the
            // suffix path must NOT start with a '/'. See
            // DKApiClient for more on the BaseAddress

            // The following is correct:        path = 'Organization'
            // The following is NOT correct:    path = '/Organization'
            const string path = "Organization";
            var response = await Client.GetAsync(path);
            
            return await response.Content.ReadAsStringAsync();
        }

        // pull the oldest entry possible TODO <config oldest date setup>
        public async Task<string> PullOldest()
        {
            // This might need some configuring from the config file.
            return await Task.Run(() => (string) null);
        }

        // tests to see if it is responsive (Needs work)
        public async Task<bool> IsResponding()
        {
            // is the api online and working? yes? good! thank god!
            const string path = "Organization";
            var response = await Client.GetAsync(path);

            if (response.StatusCode == HttpStatusCode.OK) {
                Console.WriteLine("API: I'm online!!!");
                return true;
            }
            
            Console.WriteLine("api is offline... :(");
            Console.WriteLine($"Status code received: {(int) response.StatusCode}");
            return false;
        }

        // pull more entries based on input
        public async Task<string> PullMore(int amount, string queryText, string timeStart, string timeEnd)
        {
            // pull this many entries with this specific query from time this to this
            return await Task.Run(() => (string) null);
        }
    }
}