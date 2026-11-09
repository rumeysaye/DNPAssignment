using ApiContracts;
using System.Text.Json;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client; 
 
    public HttpPostService(HttpClient client) 
    { 
        this.client = client; 
    } 
 
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse =
            await client.PostAsJsonAsync("posts", request);

        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
    
    public async Task<PostDto> GetPostAsync(int id)
    {
        PostDto? post =
            await client.GetFromJsonAsync<PostDto>($"posts/{id}");

        return post ?? throw new Exception("Post not found");
    }
    
    public async Task<IEnumerable<PostDto>> GetPostsAsync()
    {
        IEnumerable<PostDto>? posts =
            await client.GetFromJsonAsync<IEnumerable<PostDto>>("posts");

        return posts ?? Enumerable.Empty<PostDto>();
    }
    
    
} 
