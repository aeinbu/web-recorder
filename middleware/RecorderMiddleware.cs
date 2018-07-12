using System.IO;
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
			// Request recording
			context.Request.EnableBuffering();
			int filenumber = await recorder.RecordRequest(context.Request);
			context.Request.Body.Seek(0, SeekOrigin.Begin);

			// Response recording
			var originalResponseBodyStream = context.Response.Body;
			try
			{
				using (var ms = new MemoryStream())
				{
					context.Response.Body = ms;

					// Call the next delegate/middleware in the pipeline
					await _next(context);

					ms.Seek(0, SeekOrigin.Begin);
					await recorder.RecordResponse(context.Response, filenumber);
					ms.Seek(0, SeekOrigin.Begin);
					await ms.CopyToAsync(originalResponseBodyStream);
				}
			}
			finally
			{
				context.Response.Body = originalResponseBodyStream;
			}
		}
	}
}