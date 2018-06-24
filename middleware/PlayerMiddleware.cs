using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class PlayerMiddleware
	{
		private readonly RequestDelegate _next;

		public PlayerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, Player player)
		{
			var responseSent = await player.Play(context.Request, context.Response);
			if (!responseSent)
			{
				await _next(context);
			}
		}
	}

}