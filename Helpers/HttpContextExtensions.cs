using Microsoft.AspNetCore.Http;

namespace Kahveci.Helpers
{
    public static class HttpContextExtensions
    {
        public static int? GetUserId(this HttpContext context)
        {
            return context.Session.GetInt32("UserId");
        }
    }
}
