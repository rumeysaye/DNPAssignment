using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    
    Task<IEnumerable<CommentDto>> GetCommentsAsync(int postId);
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
}