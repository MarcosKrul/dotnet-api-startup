using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.Attributes
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(
                new ErrorApiResponse
                {
                    Errors = modelState
                        .Keys.SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                        .ToList()
                        .Select(item => new AppErrorDescriptor
                        {
                            Key = MessageKey.INVALID_INPUT,
                            Description = item
                        })
                }
            )
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }

    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
