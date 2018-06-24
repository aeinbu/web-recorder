using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Middleware.Storage;

namespace Middleware
{
	public class Recorder
	{
		public class Options
		{
			public bool RecordResponses { get; set; }
		}

		private readonly IRecorderStore _store;
		private readonly bool _recordResponses;
		private readonly ILogger<Recorder> _logger;

		public Recorder(IRecorderStore store, IOptions<Options> options, ILogger<Recorder> logger)
		{
			_store = store;
			_recordResponses = options.Value.RecordResponses;
			_logger = logger;
		}

		public async Task<int> RecordRequest(HttpRequest request)
		{
			var payload = new RequestPayload()
			{
				Path = request.Path.ToString(),
				Querystring = request.QueryString,
				Protocol = request.Protocol,    // "HTTP/1.1"
				Scheme = request.Scheme,        // "https" or https"
				Method = request.Method,        // "GET", "POST", "PUT", "DELETE" etc
				Headers = request.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}"),
				Body = await ResolveStreamToString(request.Body)
			};

			return await _store.Save(payload);
		}

		public async Task RecordResponse(HttpResponse response, int filenumber)
		{
			if (_recordResponses)
			{
				var payload = new ResponsePayload(){
					Protocol = response.HttpContext.Request.Protocol,
					StatusCode = response.StatusCode,
					//TODO: How to get the reason phrase?
					ReasonPhrase = response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase ?? "<Unknown reason phrase>",
					Headers = response.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}")
				};
				await _store.Save(payload, filenumber);
			}
		}

		private async Task<string> ResolveStreamToString(Stream stream)
		{
			using (var sr = new StreamReader(stream))
			{
				return await sr.ReadToEndAsync();
			}
		}
	}
}