using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using Ionic.Zip;

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

        // CodeQL code smell: hardcoded credentials
        public static bool AuthenticateUser(string username)
        {
            // BAD: Hardcoded password
            string password = "SuperSecret123!";
            // Dummy check
            return username == "admin" && password == "SuperSecret123!";
        }

        // CodeQL/code scanning: vulnerable 3rd-party dependency usage
        public static object? UnsafeDeserialize(string json)
        {
            // BAD: Deserializing untrusted input with vulnerable Newtonsoft.Json
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }

        // CodeQL/code scanning: vulnerable DotNetZip usage
        public static void UnsafeExtractZip(string zipPath, string extractPath)
        {
            // BAD: DotNetZip <1.13 vulnerable to directory traversal
            using (var zip = Ionic.Zip.ZipFile.Read(zipPath))
            {
                zip.ExtractAll(extractPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            }
        }

    }
}
