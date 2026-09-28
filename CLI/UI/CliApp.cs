using CLI.UI.ManageUsers;
using RepositoryContracts;
using CLI.UI.ManagePosts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository; 
    private  readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

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
            Console.WriteLine("1. Manage users"); 
            Console.WriteLine("2. Manage Posts"); 
            Console.WriteLine("3. Exit");
        
        //Læs fra brugerens input
        string? input = Console.ReadLine();
        
        switch (input)
        {
            case "1" : 
                Console.WriteLine("Manage User selected");
                ManageUserView manageUserView = new ManageUserView(userRepository);
                await manageUserView.ManageUserAsync();
                break;
            
            case "2" : 
                Console.WriteLine("Posts");
                ManagePostView managePostsView = new ManagePostView(postRepository,userRepository,commentRepository);
                await managePostsView.ManagePostAsync();
                break;
            
            case "3" :
                Console.WriteLine("Exit");
                return;
            
            default:
                Console.WriteLine("Invalid input");
                break; 
        } 
        }
    }
    
}