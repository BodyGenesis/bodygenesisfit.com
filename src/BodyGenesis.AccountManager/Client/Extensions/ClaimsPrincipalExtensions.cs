namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string type, string defaultValue = "")
        {
            return principal.FindFirst(type)?.Value ?? defaultValue;
        }
    }
}
