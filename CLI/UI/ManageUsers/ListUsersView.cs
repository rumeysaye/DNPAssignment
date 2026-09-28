using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void ListUsers()
    {
        var users = userRepository.GetManyAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"ID:{user.Id}, Username: {user.UserName}");
        }
    }
}