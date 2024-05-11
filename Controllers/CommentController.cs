using Microsoft.AspNetCore.Mvc;
using TucaAPI.Dtos.Comment;
using TucaAPI.Interfaces;
using TucaAPI.Mappers;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;

        public CommentController(ICommentRepository repository, IStockRepository stockRepository)
        {
            this.commentRepository = repository;
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await this.commentRepository.GetAllAsync();
            var formatted = comments.Select(x => x.ToCommentDto());

            return Ok(formatted);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await this.commentRepository.GetByIdAsync(id);

            if (comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [Route("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto data)
        {
            if (!await this.stockRepository.StockExistsAsync(stockId))
            {
                return BadRequest("Stock not found");
            }

            var comment = data.ToCommentFromRequestDto(stockId);

            await this.commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            var comment = await this.commentRepository.GetByIdAsync(id);

            if (comment == null) 
            {
                return NotFound();
            }

            await this.commentRepository.DeleteAsync(comment);

            return NoContent();
        }
    }
}