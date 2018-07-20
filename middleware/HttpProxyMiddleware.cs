using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Middleware
{
	public class HttpProxyMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<HttpProxyMiddleware> _logger;

		public HttpProxyMiddleware(RequestDelegate next, ILogger<HttpProxyMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Headers.ContainsKey("Host"))
			{
				// Make a request to requested server
				var client = new HttpClient();
				var requestMessage = CreateRequestMessage(context.Request);
				var responseMessage = await client.SendAsync(requestMessage);

				// Return the responseMessage as a response

				// responseMessage.Headers.Add("Content-Type", "text/html");
				await responseMessage.Content.CopyToAsync(context.Response.Body);
				// using (_logger.BeginScope("*** Response headers"))
				// {
				// 	foreach (var hdr in responseMessage.Headers)
				// 	{
				// 		_logger.LogDebug($"{hdr.Key}: {hdr.Value}");
				// 		context.Response.Headers.Add(hdr.Key, new StringValues(hdr.Value.ToArray()));
				// 	}
				// }
			}
			else
			{
				// Current request is not a proxy request, since it misses the "Host" header
				await _next(context);
			}
		}

		private HttpRequestMessage CreateRequestMessage(HttpRequest request)
		{
			var host = $"{request.Scheme}://{request.Headers["Host"]}";
			var method = ResolveHttpMethod(request.Method);
			var headers = request.Headers.Where(hdr => hdr.Key != "Host" && hdr.Key != "Content-type");

			var requestMessage = new HttpRequestMessage(method, host);
			// requestMessage.Headers.Add("Accept", new[] { "text/html" });

			// using (_logger.BeginScope("*** Request headers"))
			// {
			// 	foreach (var hdr in headers)
			// 	{
			// 		_logger.LogDebug($"{hdr.Key}: {hdr.Value}");
			// 		requestMessage.Headers.Add(hdr.Key, hdr.Value.AsEnumerable());
			// 	}
			// }

			return requestMessage;
		}

		private HttpMethod ResolveHttpMethod(string method)
		{
			switch (method)
			{
				case "GET": return HttpMethod.Get;
				case "POST": return HttpMethod.Post;
				case "PUT": return HttpMethod.Put;
				case "DELETE": return HttpMethod.Delete;
				case "OPTIONS": return HttpMethod.Options;
				case "PATCH": return HttpMethod.Patch;
				case "HEAD": return HttpMethod.Head;
				case "TRACE": return HttpMethod.Trace;
				default: throw new InvalidOperationException("Unknown HTTP method");
			}
		}
	}
}