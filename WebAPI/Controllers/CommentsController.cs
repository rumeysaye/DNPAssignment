using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(
        ICommentRepository commentRepo,
        IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
    }

    // CREATE COMMENT
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = new()
            {
                Body = request.Body,
                UserId = request.UserId,
                PostId = request.PostId
            };

            Comment created = await commentRepo.AddAsync(comment);

            CommentDto dto = new()
            {
                Id = created.Id,
                Body = created.Body,
                UserId = created.UserId,
                PostId = created.PostId
            };

            return Created($"/comments/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // GET SINGLE COMMENT
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(
        [FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);

            CommentDto dto = new()
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            };

            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    // GET MANY COMMENTS
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetManyComments(
        [FromQuery] int? userId,
        [FromQuery] string? username,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetManyAsync();
        IQueryable<User> users = userRepo.GetManyAsync();

        List<CommentDto> dtos = new();

        foreach (Comment comment in comments)
        {
            // Filter after userId
            if (userId.HasValue && comment.UserId != userId.Value)
            {
                continue;
            }

            // Filter after username
            if (!string.IsNullOrEmpty(username))
            {
                User? author = users.FirstOrDefault(
                    u => u.Id == comment.UserId);

                if (author == null ||
                    !author.UserName.Contains(
                        username,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            // Filter after postId
            if (postId.HasValue && comment.PostId != postId.Value)
            {
                continue;
            }

            CommentDto dto = new()
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            };

            dtos.Add(dto);
        }

        return Ok(dtos);
    }

    // UPDATE COMMENT
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] CommentDto request)
    {
        try
        {
            Comment existingComment = await commentRepo.GetSingleAsync(id);

            existingComment.Body = request.Body;
            existingComment.UserId = request.UserId;
            existingComment.PostId = request.PostId;

            await commentRepo.UpdateAsync(existingComment);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    // DELETE COMMENT
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.GetSingleAsync(id);
            await commentRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }
}