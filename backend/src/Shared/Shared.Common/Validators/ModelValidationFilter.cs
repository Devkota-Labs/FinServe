using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Application.Responses;
using Shared.Application.Results;

namespace Shared.Common.Validators;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Any())
                .SelectMany(kvp => kvp.Value is null ? [] : kvp.Value.Errors.Select(e =>
                    new ValidationError(kvp.Key, e.ErrorMessage)))
                .ToList();

            var result = Result.Validation(errors);
            context.Result = new BadRequestObjectResult(ApiResponse.FromResult(result));
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
