using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }   

    public void ListPosts()
    {
        var posts = postRepository.GetManyAsync();
        foreach (var post in posts )
        {
            Console.WriteLine($"{post.Id} - {post.Title}");
            
        }
    }
    
}