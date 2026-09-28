using CLI.UI.ManageComments;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    
    public ManagePostView(IPostRepository postRepository,IUserRepository  userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ManagePostAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Manage posts: ");
            Console.WriteLine("1. Create post ");
            Console.WriteLine("2.Add Comment");
            Console.WriteLine("3. View specific post ");
            Console.WriteLine("4. View all posts ");
            //Console.WriteLine("2. Delete post ");
            //Console.WriteLine("3. Edit post: ");
            Console.WriteLine("0. Back ");
            
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreatePostView createPostView = new CreatePostView(postRepository, userRepository);
                    await createPostView.CreatePostAsync();
                    break;
                
                case "2":
                    CreateCommentView createCommentView =
                        new CreateCommentView(commentRepository, postRepository, userRepository);
                    await createCommentView.CreateCommentAsync();
                    break;
                    
                
                case "3":
                        ViewPostView viewPostView = new ViewPostView(postRepository,commentRepository);
                        await viewPostView.ViewPostAsync();
                    break;
                
                case"4":
                    Console.WriteLine("List of posts ");
                    ListPostsView listPostsView = new ListPostsView(postRepository);
                    listPostsView.ListPosts();
                    break;
                
                /* case "5":
                  Console.WriteLine("Delete post selected: ");
                  break;
              */
                
                /* case "6":
                  Console.WriteLine("Edit post selected: ");
                  break;
              */

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            } 
        }
        
    }
    
}
    