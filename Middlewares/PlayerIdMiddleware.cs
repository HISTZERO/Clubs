using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Clubs.Middlewares
{
    public class PlayerIdMiddleware
    {
        private readonly RequestDelegate _next;

        public PlayerIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Player-ID", out var playerIdValue) &&
                int.TryParse(playerIdValue, out int playerId))
            {
                context.Items["Player-ID"] = playerId;
            }
            else
            {
                context.Items["Player-ID"] = null;
            }

            await _next(context);
        }
    }
}
