using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CashFlowException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknowError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            var cashFlowException = (CashFlowException)context.Exception;
            var erroResponse = new ResponseErrorsJson(cashFlowException.GetErrors());

            context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
            context.Result = new ObjectResult(erroResponse);
        }

        private void ThrowUnknowError(ExceptionContext context)
        {
            var erroResponse = new ResponseErrorsJson(ResourceErrorMessages.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(erroResponse);
        }
    }
}
