using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

		private readonly IRequestStore _requestStore;
		private readonly IResponseStore _responseStore;
		private readonly bool _recordResponses;
		private readonly ILogger<Recorder> _logger;

		public Recorder(IRequestStore requestStore, IOptions<Options> options, ILogger<Recorder> logger)
		{
			_requestStore = requestStore;
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

			return await _requestStore.Save(payload);
		}

		public async Task RecordResponse(HttpResponse response, int filenumber)
		{
			if (_recordResponses)
			{
				var payload = new ResponsePayload(){
					Protocol = "//TODO: PROTOCOL", //response.Protocol,
					StatusCode = response.StatusCode,
					StatusMessage = "TODO:// STATUSMESSAGE", //response.StatusMessage,
					Headers = response.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}")
				};
				await _requestStore.Save(payload, filenumber);
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