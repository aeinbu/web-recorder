using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class HttpProxyMiddleware
	{
		private readonly RequestDelegate _next;

		public HttpProxyMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			// Make a request to requested server
			var client = new HttpClient();
			var requestMessage = CreateRequestMessage(context.Request);
			var responseMessage = await client.SendAsync(requestMessage);

			// Return the responseMessage as a response
			await responseMessage.Content.CopyToAsync(context.Response.Body);

			//TODO: Copy all other response headers
		}

		private HttpRequestMessage CreateRequestMessage(HttpRequest request)
		{
			var host = $"{request.Scheme}://{request.Headers["Host"]}";
			var method = ResolveHttpMethod(request.Method);
			var requestMessage = new HttpRequestMessage(method, host);

			//TODO: copy all other request headers

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