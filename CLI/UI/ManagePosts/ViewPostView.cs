using RepositoryContracts;
using Entities;
namespace CLI.UI.ManagePosts;

public class ViewPostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository  commentRepository;
    
    public ViewPostView(IPostRepository postRepository,ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ViewPostAsync()
    {
        Console.WriteLine("Enter post ID: ");
        string? postInput = Console.ReadLine();
        
        
        if (!int.TryParse(postInput, out int postId))
        {
            Console.WriteLine("ID must be a number");
            return;
        }
        try
        {
            Post post = await postRepository.GetSingleAsync(postId);
            Console.WriteLine();
            Console.WriteLine($"Post: {post.Id}");
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");
            
            Console.WriteLine();
            Console.WriteLine("Comments:");


            var comments = commentRepository.GetManyAsync();
            foreach (var comment in comments)
            {
                if (post.Id == comment.PostId)
                {
                    Console.WriteLine($"{comment.Id} - {comment.Body}");
                }
            }
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Invalid post ID.");
       
        }
    }
}