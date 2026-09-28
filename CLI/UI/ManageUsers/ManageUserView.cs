using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUserView
{
    private readonly IUserRepository userRepository;

    public ManageUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ManageUserAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Manage Users");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("0. Back");


            string? input = Console.ReadLine();


            switch (input)
            {
                case "1":
                    Console.WriteLine("Create User selected");
                    CreateUserView createUserView = new CreateUserView(userRepository);
                    await createUserView.CreateUserAsync();
                    break;

                case "2":
                    Console.WriteLine("List of users");
                    ListUsersView lisUsersView = new ListUsersView(userRepository);
                    lisUsersView.ListUsers();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}