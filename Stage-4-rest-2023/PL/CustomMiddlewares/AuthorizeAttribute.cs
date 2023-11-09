using Microsoft.AspNetCore.Authorization;
using Stage4rest2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stage4rest2023.Exceptions;


namespace Stage4rest2023.Services;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            throw new TokenValidationException("Opnieuw inloggen vereist");
        }
    }
}