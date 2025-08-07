using System.Net.Http.Headers;
using System.Text.Json;

namespace WebAPIClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // JsonSerializer = class used to deserialize JSON into C# objects

            // 1
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
            // 2
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

            // 3
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json);
            posts.ForEach(Console.WriteLine);

            // 4
            var usersUrl = "https://jsonplaceholder.typicode.com/users";
            var c5 = new HttpClient();
            string usersJson = await c5.GetStringAsync(usersUrl);
            string postsJson = await c5.GetStringAsync(url2);

            var users = JsonSerializer.Deserialize<List<User>>(usersJson);
            var _posts = JsonSerializer.Deserialize<List<Post>>(postsJson);
            foreach(var post in _posts)
            {
                var user = users.FirstOrDefault(u => u.id == post.userId);
                string uname = user?.name ?? "Unknown";
                Console.WriteLine($"{post.id} {uname} {post.title}");
            }

            // 5
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
        }
    }
}