using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TucaAPI.Data;
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

        public async Task<List<Comment>> GetAllAsync()
        {
            return await context.Comments.ToListAsync();
        }
    }
}