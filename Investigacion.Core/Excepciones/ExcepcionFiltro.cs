using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Investigacion.Core.Excepciones {
    public class ExcepcionFiltro : IExceptionFilter {
        public void OnException(ExceptionContext context) {
            if (context.Exception.GetType() == typeof(ExcepcionCore)) {

                // Determina tipo de excepcion
                var Exception = (ExcepcionCore)context.Exception;

                // Cuerpo de excepcion
                var CuerpoExcepcion = new {
                    status = 400,
                    title = "Bad request",
                    detail = Exception.Message
                };

                // estructura de transferencia de excepcion
                var Json = new {
                    errors = new[] { CuerpoExcepcion }
                };

                // Configuraciones de Excepcion
                context.Result = new BadRequestObjectResult(Json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            }


            if (context.Exception.GetType() == typeof(NotFoundExcepcionCore)) {

                // Determina tipo de excepcion
                var Exception = (NotFoundExcepcionCore)context.Exception;

                // Cuerpo de excepcion
                var CuerpoExcepcion = new {
                    status = 404,
                    title = "Not Found",
                    detail = Exception.Message
                };

                // estructura de transferencia de excepcion
                var Json = new {
                    errors = new[] { CuerpoExcepcion }
                };

                // Configuraciones de Excepcion
                context.Result = new BadRequestObjectResult(Json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            }
        }
    }
}
