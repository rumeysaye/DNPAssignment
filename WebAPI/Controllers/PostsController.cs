using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            Post post = new()
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId
            };
            Post created = await postRepo.AddAsync(post);
            PostDto dto = new()
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };
            return Created($"/posts/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSinglePost([FromRoute] int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);

            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };

            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetManyPosts(
        [FromQuery] string? title,
        [FromQuery] int? userId,
        [FromQuery] string? username)
    {
        IQueryable<Post> posts = postRepo.GetManyAsync();
        IQueryable<User> users = userRepo.GetManyAsync();

        List<PostDto> dtos = new();

        foreach (Post post in posts)
        {
            //Filter after title
            if (!string.IsNullOrEmpty(title) &&
                !post.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            //Filter after userId
            if (userId.HasValue && post.UserId != userId.Value)
            {
                continue;
            }

            // Filter after username
            if (!string.IsNullOrEmpty(username))
            {
                User? author = users.FirstOrDefault(u => u.Id == post.UserId);

                if (author == null ||
                    !author.UserName.Contains(username, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            dtos.Add(dto);
        }

        return Ok(dtos);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(
        [FromRoute] int id,
        [FromBody] PostDto request)
    {
        try
        {
            Post existingPost = await postRepo.GetSingleAsync(id);

            existingPost.Title = request.Title;
            existingPost.Body = request.Body;
            existingPost.UserId = request.UserId;

            await postRepo.UpdateAsync(existingPost);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepo.GetSingleAsync(id);
            await postRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }
}

