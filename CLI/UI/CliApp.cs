using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private IUserRepository userRepository; 
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }
    
    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine(); 
            Console.WriteLine("Welcome to RedditRum"); 
            Console.WriteLine("1. Create User"); 
            Console.WriteLine("2. View Posts"); 
            Console.WriteLine("3. Exit");
        
        //Læs fra brugerens input
        string? input = Console.ReadLine();

        if (input == "1")
        {
            Console.WriteLine("Create User selected");
        }
        else if (input == "2")
        {
            Console.WriteLine("View Posts selected");
        }
        else if (input == "3")                         
        {                                              
            Console.WriteLine("Exit");  
        }                                              
        else if  (input == "0")
        {
            break;
        }
        else 
        {
            Console.WriteLine("Invalid input");
        } 
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("Enter username: ");
        
        string? username  = Console.ReadLine();
        
        Console.WriteLine("Enter password: ");
        
        string? password = Console.ReadLine();
    }
}