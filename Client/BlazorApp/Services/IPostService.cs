using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> GetPostAsync(int id);
    public Task<IEnumerable<PostDto>> GetPostsAsync();
    public Task<PostDto> AddPostAsync(CreatePostDto request);

// ... more methods 
}