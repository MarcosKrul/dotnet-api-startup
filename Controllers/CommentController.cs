using Microsoft.AspNetCore.Mvc;
using TucaAPI.Interfaces;
using TucaAPI.Mappers;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository repository;

        public CommentController(ICommentRepository repository)
        {
            this.repository = repository;   
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var comments = await this.repository.GetAllAsync();
            var formatted = comments.Select(x => x.ToCommentDto());

            return Ok(formatted);
        }
    }
}