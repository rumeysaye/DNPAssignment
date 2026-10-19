using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    
    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }
}