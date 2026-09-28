using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository,  IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task CreatePostAsync()
    {
        Console.WriteLine("Enter title for your new post: ");
        string? postTitle = Console.ReadLine();

        Console.WriteLine("Enter description for your new post: ");
        string? postBody = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(postTitle) || 
            string.IsNullOrWhiteSpace(postBody))
        {
            Console.WriteLine("Post title or Post description is empty, you must enter both!");
            return;
        }
        
        Console.WriteLine("Enter User ID: ");
        string? userIdInput = Console.ReadLine();
        //Metoden forsørger at konvertere string til int
        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Invalid input: User ID must be a number!");
            return;
        }
        
        //tjekker om UserId eksistere
        try
        {
            await userRepository.GetSingleAsync(userId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("User does not exist!");
            return;
        }
        
        Entities.Post post = new()
        {
            Title = postTitle,
            Body = postBody,
            UserId = userId
        };

        await postRepository.AddAsync(post);
        
        Console.WriteLine("Post created successfully!");
        
    }
}