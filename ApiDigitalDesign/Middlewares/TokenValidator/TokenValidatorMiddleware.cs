using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDigitalDesign.Services;
using Common.Exceptions.Auth;
using Common.Exceptions.Session;
using Common.Exceptions.User;
using Common.Generics;
using Common.Statics;
using DAL.Entities;

namespace ApiDigitalDesign.Middlewares.TokenValidator
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context, SessionService sessionService, UserService userService)
        {
            if (!context.User.Identity!.IsAuthenticated) await _next(context);
            else
            {
                try
                {
                    var isOk = true;
                    var sessionId = context.User.Claims.GetClaimValueOrDefault<Guid>(Auth.SessionClaim);
                    var userId = context.User.Claims.GetClaimValueOrDefault<Guid>(Auth.UserClaim);
                    if (userId == default) throw new InvalidTokenException("invalid userId");
                    if (sessionId != default)
                    {
                        var user = await userService.GetUserByIdAsync(userId);
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
                catch (Exception ex) when (ex is InvalidTokenException ||
                                           ex is UserNotFoundException ||
                                           ex is SessionNotFoundException)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                }
            }
        }

    }
}
