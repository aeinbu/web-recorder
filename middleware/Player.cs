using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Storage;

namespace Middleware
{
	public class Player
	{
		private readonly RequestDelegate _next;

		public Player(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IResponseStore storage)
		{
			using (Stream responseStream = await storage.Load(context.Request.Method, context.Request.Path))
			{
				if (responseStream == null)
				{
					//TODO: Change this to a seperate middleware? Or use existing one?
					await context.Response.WriteAsync("Thank you! Your request was recorded...");

					// await _next(context);
					return;
				}

				responseStream.CopyTo(context.Response.Body);
			}
		}
	}
}