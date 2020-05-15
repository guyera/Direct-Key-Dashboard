using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

// Essentially middleware which configures messages
// before they are sent to the API, such as
// by attaching the required certificate.
namespace InformationLibraries
{
    public class DKApiMessageHandler : HttpClientHandler
    {
        private const string EnvironmentVariableDKApiUsername = "DK_API_USERNAME";
        private const string EnvironmentVariableDKApiPassword = "DK_API_PASSWORD";
        private const string EnvironmentVariableDKApiCertPath = "DK_API_CERT_PATH";
        private const string EnvironmentVariableDKApiCertPass = "DK_API_CERT_PASS";
        
        // Http Basic Auth credentials and client ssh certificate path
        private readonly AuthenticationHeaderValue AuthHeaderValue;
        
        public DKApiMessageHandler() {
            var username = Environment.GetEnvironmentVariable(EnvironmentVariableDKApiUsername);
            var password = Environment.GetEnvironmentVariable(EnvironmentVariableDKApiPassword);
            AuthHeaderValue = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                        $"{username}:{password}")));
            
            // Use client ssh certificate
            var certPath = Environment.GetEnvironmentVariable(EnvironmentVariableDKApiCertPath);
            var certPass = Environment.GetEnvironmentVariable(EnvironmentVariableDKApiCertPass);
            var cert = new X509Certificate2(certPath, certPass);
            ClientCertificates.Add(cert);
        }

        // Override SendAsync and append auth header before sending request
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = AuthHeaderValue;
            return await base.SendAsync(request, cancellationToken);
        }
    }
}