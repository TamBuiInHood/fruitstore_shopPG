using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ErrorWrappingMiddlewares
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddlewares(ILogger logger, RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(Log));
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var errorMsg = string.Empty;
            try
            {
                await _next.Invoke(context);
            } catch (ValidationException ex)
            {

            }
        }
    }
}
