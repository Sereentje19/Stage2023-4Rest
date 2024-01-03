using System.Security.Claims;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PL.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        /// <summary>
        /// Performs authorization checks based on the presence of the [AllowAnonymous] attribute
        /// and the authentication status of the current user.
        /// </summary>
        /// <param name="context">The AuthorizationFilterContext providing information about the authorization context.</param>
        /// <exception cref="TokenValidationException">
        /// Thrown when authorization fails, and the user is not authenticated.
        /// </exception>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            ClaimsPrincipal user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new TokenValidationException("Opnieuw inloggen vereist");
            }
        }
    }
}
