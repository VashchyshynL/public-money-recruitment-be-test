using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VacationRental.Application.Common.Exceptions;

namespace VacationRental.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ConflictException), HandleConflictException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(exceptionType))
            {
                _exceptionHandlers[exceptionType].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            const string errorMessage = "An error occurred while processing the request";

            context.Result = new ObjectResult(errorMessage)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private static void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;
            var errorDetails = string.Join(Environment.NewLine, exception?.Errors.Values.SelectMany(x => x) ?? Array.Empty<string>());
            context.Result = new BadRequestObjectResult(errorDetails);

            context.ExceptionHandled = true;
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new NotFoundObjectResult(exception?.Message);

            context.ExceptionHandled = true;
        }
        
        private static void HandleConflictException(ExceptionContext context)
        {
            var exception = context.Exception as ConflictException;

            context.Result = new ConflictObjectResult(exception?.Message);

            context.ExceptionHandled = true;
        }
    }
}
