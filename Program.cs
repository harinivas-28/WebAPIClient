using System.Net.Http.Headers;
using System.Text.Json;

namespace FreeApiJsonDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            await ProcessRepoAsync(client);
            static async Task ProcessRepoAsync(HttpClient client)
            {
                await using Stream stream = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
                var repos = await JsonSerializer.DeserializeAsync<List<Repo>>(stream);
                Console.WriteLine(repos);
                // below loop says that is repos exists, then iterate through each repo
                foreach (var repo in repos ?? Enumerable.Empty<Repo>())
                {
                    Console.WriteLine(repo);
                }
            }

            // JsonSerializer = class used to deserialize JSON into C# objects

            // 2
            string url = "https://jsonplaceholder.typicode.com/posts/1";
            using HttpClient client2 = new();
            try
            {
                HttpResponseMessage res = await client2.GetAsync(url);
                res.EnsureSuccessStatusCode();
                string resBody = await res.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Respose:");
                Console.WriteLine(resBody);
            } catch(HttpRequestException e)
            {
                Console.WriteLine("Error fetching data:");
                Console.WriteLine(e.Message);
            }
        }
    }
}