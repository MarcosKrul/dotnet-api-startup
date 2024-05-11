
using TucaAPI.Models;

namespace TucaAPI.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        
    }
}