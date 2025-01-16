using InteriorCoffee.Application.Enums.Account;
using InteriorCoffee.Application.Utils;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace InteriorCoffeeAPIs.Validate
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(params AccountRoleEnum[] roles)
        {
            var allowRoleAsString = roles.Select(role => role.GetEnumDescription()).ToArray();
            Roles = string.Join(",", allowRoleAsString);
        }
    }
}
