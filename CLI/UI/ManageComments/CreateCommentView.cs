using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    
    public CreateCommentView(ICommentRepository commentRepository,
        IPostRepository postRepository,
        IUserRepository userRepository)
        {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        }

    public async Task CreateCommentAsync()
    {   //Indholdet af kommentaren
        Console.WriteLine("Enter your comment");
        string? commentBody = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(commentBody))
        {
            Console.WriteLine("Comment cannot be empty");
            return;
        }
        
        //Hvilken user skriver kommentaren?
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
        
        //Brugeren vælger hvilet post kommentaren tilhører
        Console.WriteLine("Enter Id for the post you want to comment on: ");
        string? postIdInput = Console.ReadLine();
        
        if (!int.TryParse(postIdInput, out int postId))
        {
            Console.WriteLine("Invalid input: Post ID must be a number!");
            return;
        }
        
        //tjekker om PostId eksistere
        try
        {
            await postRepository.GetSingleAsync(postId);
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Post does not exist!");
            return;
        }
        
        //Opretter kommentaren
        Entities.Comment comment = new()
        {
            Body = commentBody,
            UserId = userId,
            PostId = postId
           
        };
        
        //Gemmer kommentaren
        await commentRepository.AddAsync(comment);
        
        Console.WriteLine("Comment created successfully!");
        
    }
}