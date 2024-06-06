using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.Attributes;
using TucaAPI.Dtos.Comment;
using TucaAPI.Extensions;
using TucaAPI.Interfaces;
using TucaAPI.Mappers;
using TucaAPI.Models;
using TucaAPI.src.Common;

namespace TucaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;
        private readonly UserManager<AppUser> userManager;

        public CommentController(
            ICommentRepository repository,
            IStockRepository stockRepository,
            UserManager<AppUser> userManager
        )
        {
            this.commentRepository = repository;
            this.stockRepository = stockRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetAll()
        {
            var comments = await this.commentRepository.GetAllAsync();
            var formatted = comments.Select(i => i.ToCommentDto());

            return Ok(formatted);
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await this.commentRepository.GetByIdAsync(id);

            if (comment is null) return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto data)
        {
            if (!await this.stockRepository.StockExistsAsync(stockId))
                return BadRequest(Messages.STOCK_NOT_FOUND);

            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser is null) return Unauthorized();

            var comment = data.ToCommentFromRequestDto(stockId, hasUser.Id);

            await this.commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser is null) return Unauthorized();

            var comment = await this.commentRepository.GetByIdAsync(id);

            if (comment is null) return NotFound();

            if (comment.AppUserId != hasUser.Id) return Unauthorized();

            await this.commentRepository.DeleteAsync(comment);

            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto data)
        {
            var email = User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email ?? "");
            if (hasUser is null) return Unauthorized();

            var comment = await this.commentRepository.UpdateAsync(id, data, hasUser);

            if (comment is null) return NotFound();

            return Ok(comment.ToCommentDto());
        }
    }
}