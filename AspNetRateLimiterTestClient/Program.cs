using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetRateLimiterTestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpClient = GetClient();
            int TotalRuns = 0;
            while (TotalRuns < 1000)
            {
                try
                {
                    // Make an HTTP GET request
                    HttpResponseMessage response = httpClient.GetAsync("WeatherForecast").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                TotalRuns++;
                Task.Delay(100).Wait();
                Console.WriteLine($"{TotalRuns} - WeatherForecast API calls completed");

            }
            Console.ReadKey();
            // Get client
            HttpClient GetClient()
            {
                var services = new ServiceCollection();
                services.AddHttpClient("RateLimiterExampleApi", client =>
                {
                    client.BaseAddress = new Uri("https://localhost:5157");
                });

                // Build the service provider and get an instance of HttpClient
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                return httpClientFactory.CreateClient("RateLimiterExampleApi");

            }

        }

    }

}
