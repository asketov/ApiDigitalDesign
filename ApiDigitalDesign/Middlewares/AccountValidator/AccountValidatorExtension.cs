using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDigitalDesign.Middlewares.AccountValidator
{
    public static class AccountValidatorExtension
    {
        public static IApplicationBuilder UseAccountValidator(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccountValidatorMiddleware>();
        }
    }
}
