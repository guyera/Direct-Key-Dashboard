using System;
using System.Collections.Generic;

// Used to cache responses from the API.
namespace InformationLibraries
{
    public class Storage 
    {
        // Database Contrained from queries.
        private Dictionary<string, string> cacheDB = new Dictionary<string, string>();

        // takes key and pulls data if it's there otherwise it returns ""
        public string PullFromDatabase(string key)
        {
            // the reason for this is to check to see if the key exists, and if it doesn't then pull nothing.
            if (!cacheDB.TryGetValue(key, out string result)) { result = ""; }
            return result;
        }

        // This will need lots of refactoring
        // takes key and the json data as the second parameter and puts it in the database, if it is overwritten, then this returns true otherwise it returns false.
        public bool PutToDatabase(string key, string rawdata)
        {
            bool wasReplaced = false;
            if (cacheDB.ContainsKey(key)) { wasReplaced = true; }
            cacheDB[key] = rawdata;
            return wasReplaced;
        }

        // TODO take apart the Json and querie the actual API periodically and insert the json objects individually as keys, 
        // then remove the connection the user has to the database entirely, spitting back the data from the cache instead.
    }
}
