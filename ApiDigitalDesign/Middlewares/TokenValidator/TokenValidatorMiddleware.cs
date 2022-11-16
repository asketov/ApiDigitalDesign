using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDigitalDesign.Services;
using Common.Exceptions.Session;
using Common.Exceptions.User;
using Common.Generics;
using Common.Statics;

namespace ApiDigitalDesign.Middlewares.TokenValidator
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context, SessionService sessionService)
        {
            try
            {
                var isOk = true;
                var sessionId = context.User.Claims.GetClaimValueOrDefault<Guid>(Auth.SessionClaim);
                if (sessionId != default)
                {
                    var session = await sessionService.GetSessionById(sessionId);
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
            catch(SessionNotFoundException)
            {
                context.Response.Clear();
                context.Response.StatusCode = 401;
            }
        }

    }
}
