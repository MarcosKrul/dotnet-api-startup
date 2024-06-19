using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TucaAPI.src.Attributes;
using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Services.Comment;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public CommentController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> GetAll()
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<GetAllCommentService>();
                var result = await service.ExecuteAsync();
                return Ok(result);
            }
        }

        [HttpGet]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<GetCommentByIdService>();
                var result = await service.ExecuteAsync(new GetCommentByIdRequestDto { Id = id });
                return Ok(result);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                data.AggregateUser(User);
                var service = scope.ServiceProvider.GetRequiredService<CreateCommentService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpDelete]
        [Authorize]
        [ValidateModelState]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var data = new DeleteCommentRequestDto { Id = id };
                data.AggregateUser(User);
                var service = scope.ServiceProvider.GetRequiredService<DeleteCommentService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }

        [HttpPut]
        [Authorize]
        [ValidateModelState]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequestDto data)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                data.AggregateUser(User);
                var service = scope.ServiceProvider.GetRequiredService<UpdateCommentService>();
                var result = await service.ExecuteAsync(data);
                return Ok(result);
            }
        }
    }
}
