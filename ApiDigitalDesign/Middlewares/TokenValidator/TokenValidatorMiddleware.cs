using Common.Exceptions.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Middlewares.TokenValidator
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var refreshId =Guid.Parse(context.User.Claims
                .FirstOrDefault(cl => cl.Type == "refreshId").Value);
            await _next(context);
        }

    }
}
