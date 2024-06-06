using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
using TucaAPI.Dtos.Comment;
using TucaAPI.Interfaces;
using TucaAPI.Models;

namespace TucaAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext context;

        public CommentRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await this.context.Comments.AddAsync(comment);
            await this.context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Comment comment)
        {
            this.context.Remove(comment);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await this.context.Comments.Include(i => i.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await this.context.Comments.Include(i => i.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto data, AppUser user)
        {
            var comment = await this.GetByIdAsync(id);
            if (comment is null || comment.AppUserId != user.Id) return null;

            comment.Title = data.Title;
            comment.Content = data.Content;

            await this.context.SaveChangesAsync();

            return comment;
        }
    }
}