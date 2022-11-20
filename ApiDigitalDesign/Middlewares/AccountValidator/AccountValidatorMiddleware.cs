using ApiDigitalDesign.Services;
using Common.Exceptions.Auth;
using Common.Exceptions.Session;
using Common.Exceptions.User;
using Common.Generics;
using Common.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Middlewares.AccountValidator
{
    public class AccountValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public AccountValidatorMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context, UserService userService)
        {
            if (!context.User.Identity!.IsAuthenticated) await _next(context);
            else
            {
                var isDeleted = (bool) context.Items.FirstOrDefault(u => u.Key.ToString() == "isDeleted").Value!;
                if (isDeleted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 403;
                }
                else await _next(context);
            }
        }
    }
}
