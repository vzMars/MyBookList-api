using MyBookListAPI.Dto;
using MyBookListAPI.Interfaces;
using System.Text.Json;

namespace MyBookListAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        public const string BASEURL = "";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public BookRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ICollection<Volume>> SearchBooks(string query)
        {
            string searchQuery = query.Replace(" ", "%20");
            string url = $"https://books.googleapis.com/books/v1/volumes?q={searchQuery}&maxResults=40&key={_config["GoogleBooks:ServiceApiKey"]}";

            var response = await _httpClient.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SearchResult>(json)!;

                if (result.Items == null)
                    throw new Exception();

                return result.Items;
            }
            catch
            {
                return Array.Empty<Volume>();
            }
        }
    }
}
