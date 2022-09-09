using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace HttpClientWithPollyExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";

            // Create HTTP Policy to retry 3 times
            IAsyncPolicy<HttpResponseMessage> httpWaitAndpRetryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            // Use the retry policy to wrap the HTTP call
            var response = await httpWaitAndpRetryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
