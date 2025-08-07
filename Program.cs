using System.Net.Http.Headers;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ProcessRepoAsync(client);
static async Task ProcessRepoAsync(HttpClient client)
{
    var json = await client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");
    Console.WriteLine(json);
}

// JsonSerializer = class used to deserialize JSON into C# objects
