using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
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
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await this.commentRepository.GetByIdAsync(id);

            if (comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [ValidateModelState]
        [Route("{stockId:int}")]
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
        [Route("{id:int}")]
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

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto data)
        {
            var comment = await this.commentRepository.UpdateAsync(id, data);

            if (comment == null) return NotFound();

            return Ok(comment.ToCommentDto());
        }
    }
}