using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDigitalDesign.Services;
using Common.Statics;

namespace ApiDigitalDesign.Middlewares.TokenValidator
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context, SessionService _sessionService)
        {
            var isOk = true;
            var sessionIdString = context.User.Claims.FirstOrDefault(x => x.Type == Auth.SessionClaim)?.Value;
            if (Guid.TryParse(sessionIdString, out var sessionId))
            {
                var session = await _sessionService.GetSessionById(sessionId);
                if (!session.IsActive)
                {
                    isOk = false;
                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                }
            }
            if (isOk)
            {
                await _next(context);
            }

        }

    }
}
