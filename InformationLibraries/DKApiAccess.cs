using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web;

// Public facing, dependency-injected service
// used to retrieve data from the API. Base endpoint
// specified in DKApiClient.
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
            const string path = "KeyDeviceActivity";
            return await PullMore(2000, path, string.Empty, entryDateStart, entryDateEnd);
        }

        public async Task<string> PullNewest()
        {
            // NOTE: In order for the BaseAddress of the Client to work,
            // the BaseAddress must end with a '/' and the
            // suffix path must NOT start with a '/'. See
            // DKApiClient for more on the BaseAddress

            // The following is correct:        path = 'Organization'
            // The following is NOT correct:    path = '/Organization'


            // Grabs the current date minus one month and spits out a string that is usable for the API
            const string path = "KeyDeviceActivity?tranDateStart=";

            // If you want to check the date in the console uncomment the code below and uncomment the globalization library
            /*
              CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
              DateTimeFormatInfo dateTimeFormatInfo = culture.DateTimeFormat;
              dateTimeFormatInfo.DateSeparator = ":";
            */
            DateTime oneMonthAgo = (DateTime.Now).AddMonths(-1);

            var response = await Client.GetAsync(path+oneMonthAgo+"&takes=1");
            return await response.Content.ReadAsStringAsync();
        }

        // pull the oldest entry possible TODO <config oldest date setup>
        public async Task<string> PullOldest()
        {
            const string path = "KeyDeviceActivity?tranDateStart=";

            // If you want to check the date in the console uncomment the code below and uncomment the globalization library
            /*
              CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
              DateTimeFormatInfo dateTimeFormatInfo = culture.DateTimeFormat;
              dateTimeFormatInfo.DateSeparator = ":";
            */
            DateTime startTime = (DateTime.Now).AddDays(-32);
            DateTime endTime = (DateTime.Now).AddDays(-31);
            var response = await Client.GetAsync(path + startTime + $"&tranDateEnd={endTime}&takes=1");
            // This might need some configuring from the config file.
            return await response.Content.ReadAsStringAsync();
        }

        // tests to see if it is responsive (Needs work)
        public async Task<bool> IsResponding()
        {
            // is the api online and working? yes? good! thank god!
            const string path = "Organization";
            var response = await Client.GetAsync(path);

            if (response.StatusCode == HttpStatusCode.OK) {
                return true;
            }
            
            Console.WriteLine("api is offline... :(");
            Console.WriteLine($"Status code received: {(int) response.StatusCode}");
            return false;
        }

        // pull more entries based on input
        // TODO more dummy proofing
        public async Task<string> PullMore(int amount, string queryText, string parameters, DateTime? timeStart, DateTime? timeEnd)
        {
            string startpath = $"{queryText}?{parameters}";
            if (timeStart == null || timeEnd == null)
            {
                var otherresponse = await Client.GetAsync(startpath + $"{(string.IsNullOrEmpty(parameters) ? "" : "&")}takes={amount}");
                return await otherresponse.Content.ReadAsStringAsync();
            }
            startpath += "tranDateStart=";
            string endpath = "&tranDateEnd=";
            // If you want to check the date in the console uncomment the code below and uncomment the globalization library
            /*
              CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
              DateTimeFormatInfo dateTimeFormatInfo = culture.DateTimeFormat;
              dateTimeFormatInfo.DateSeparator = ":";
            */

            var response = await Client.GetAsync(startpath + HttpUtility.UrlEncode(timeStart.ToString()) + endpath + HttpUtility.UrlEncode(timeEnd.ToString()) + $"&takes={amount}");
            // pull this many entries with this specific query from time this to this
            return await response.Content.ReadAsStringAsync();
        }
    }
}