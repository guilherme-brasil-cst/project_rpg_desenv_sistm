using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RPG_BD.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var problem = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ocorreram erros de validação nos dados enviados."
            };

            context.Result = new BadRequestObjectResult(problem);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Não precisa fazer nada após a execução
    }
}