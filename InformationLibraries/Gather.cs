/*
// this is the library to grab data from the API
using System;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
// This is a public library of functions that query the API and send data back to the program.

/* Env Vars needed
 * AUTH = 
 * username = 
 * password = 
 */
 /*
namespace CachingSystem
{
    public class Gather
    {
        private const string url = "https://dkintapi.keytest.net";
        private const string certpath = @"C:\Users\TEST\Documents\Cert\CGuillory@dkintapi.keytest.net.pfx";
        private const string certpass = "egF3wO9h2F-YPZK";
        private const string auth = "Basic Q0d1aWxsb3J5OjU2Q25OOC0/N2F0LlxEdg==";
        // pull the newest entry possible
        public static string PullNewest()
        {
            const string urlparams = "/api/ver6/Organization";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+urlparams);
            
            //WebRequest request = WebRequest.Create(url + urlparams);
            request.Credentials = CredentialCache.DefaultCredentials; // TODO get creds
            request.Headers.Add("Authorization", auth);
            X509Certificate2 certificate = new X509Certificate2(certpath, certpass);
            request.ClientCertificates.Add(certificate);
            //DefaultRequestHeaders.Authorization = new AuthorizationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("Status: " + response.StatusDescription);

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            // return Most.recent.json
            
            return responseFromServer;
        }

        // pull the oldest entry possible TODO <config oldest date setup>
        public static string PullOldest()
        {
            // This might need some configuring from the config file.
            return null;
        }

        // tests to see if it is responsive (Needs work)
        public static bool IsResponding()
        {
            // is the api online and working? yes? good! thank god!
            bool isonline = false;
            const string urlparams = "/api/ver6/Organization";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + urlparams);
            //WebRequest request = WebRequest.Create(url + urlparams);
            request.Credentials = CredentialCache.DefaultCredentials; // TODO get creds
            request.Headers.Add("Authorization", auth);
            X509Certificate2 certificate = new X509Certificate2(certpath, certpass);
            request.ClientCertificates.Add(certificate);
            //DefaultRequestHeaders.Authorization = new AuthorizationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusDescription == "OK") {
                Console.WriteLine("API: I'm online!!!");
                isonline = true;
            } else {
                Console.WriteLine("api is offline... :(");
            }
            response.Close();
            return isonline;
        }

        // pull more entries based on input
        public static string PullMore(int amount, string queryText, string timeStart, string timeEnd)
        {
            // pull this many entries with this specific query from time this to this
            return null;
        }
    }
}
******/