using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Models;

namespace TucaAPI.src.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto data, AppUser user);
    }
}
