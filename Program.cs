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
                // below loop says that if repos exists, then iterate through each repo
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
                Console.WriteLine("Type of Response we got: "+res.GetType());
                Console.WriteLine("Result Content Type: "+res.Content.GetType());
                string resBody = await res.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Respose:");
                Console.WriteLine(resBody);
            } catch(HttpRequestException e)
            {
                Console.WriteLine("Error fetching data:");
                Console.WriteLine(e.Message);
            }
            // 3
            // Storing into a collection
            string url2 = "https://jsonplaceholder.typicode.com/posts";
            using HttpClient c3 = new();
            string json = await c3.GetStringAsync(url2);
            List<string> rawData = [];
            using JsonDocument doc = JsonDocument.Parse(json);
            Console.WriteLine("JSON Document Type: "+doc.GetType());
            foreach(JsonElement ele in doc.RootElement.EnumerateArray())
            {
                rawData.Add(ele.GetRawText());
            }
            Console.WriteLine(rawData[0]);
            // 4
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json);
            posts.ForEach(Console.WriteLine);
        }
    }
}