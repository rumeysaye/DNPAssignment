using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
    {
        Console.WriteLine("Enter username: ");
        string? username = Console.ReadLine();

        Console.WriteLine("Enter password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username or password is required");
            return; 
        }

        Entities.User user = new()
        {
            UserName = username,
            Password = password
        };
        
        await userRepository.AddAsync(user);
        
        Console.WriteLine("User created successfully!");
    }
}