using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class RecorderMiddleware
	{
		private RequestDelegate _next;

		public RecorderMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, Recorder recorder)
		{
			int filenumber = await recorder.RecordRequest(context.Request);

			// Call the next delegate/middleware in the pipeline
			await _next(context);

			await recorder.RecordResponse(context.Response, filenumber);
		}
	}
}