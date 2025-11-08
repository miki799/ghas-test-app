using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;

namespace DotnetGhasDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting the GHAS demo app...");

            // Example API call using RestSharp
            var client = new RestClient("https://api.agify.io");
            var request = new RestRequest("?name=michael", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful && response.Content != null)
            {
                var json = JObject.Parse(response.Content);
                var name = json["name"]?.ToString();
                var age = json["age"]?.ToObject<int>();
                var count = json["count"]?.ToObject<int>();

                Log.Information("API returned name={Name}, age={Age}, count={Count}", name, age, count);
            }
            else
            {
                Log.Error("API call failed: {StatusCode}", response.StatusCode);
            }

            Log.Information("Demo complete.");
        }
    }
}
