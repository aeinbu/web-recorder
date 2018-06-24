using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Storage;

namespace Middleware
{
	public class Recorder
	{
		private readonly IRequestStore _store;
		private readonly ILogger<Recorder> _logger;

		public Recorder( IRequestStore store, ILogger<Recorder> logger)
		{
			_store = store;
			_logger = logger;
		}

		public async Task RecordRequest(HttpRequest request)
		{
			var path = request.Path.ToString();
			var querystring = request.QueryString;
			var protocol = request.Protocol;    // "HTTP/1.1"
			var scheme = request.Scheme;        // "https" or https"
			var method = request.Method;        // "GET", "POST", "PUT", "DELETE" etc
			var headers = request.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}");
			var body = await ResolveStreamToString(request.Body);

			var payload = new List<string>();

			payload.Add($"{method} {scheme}://servername:port{path}{querystring} {protocol}");
			payload.AddRange(headers);
			payload.Add("");
			payload.Add(body);
			await _store.Save(payload);
		}

		public async Task RecordResponse(HttpResponse response)
		{
			//TODO: ...
			// var path = response.Path.ToString();
			// var querystring = response.QueryString;
			// var protocol = response.Protocol;    // "HTTP/1.1"
			// var scheme = response.Scheme;        // "https" or https"
			// var method = response.Method;        // "GET", "POST", "PUT", "DELETE" etc
			var headers = response.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}");
			// var body = await ResolveStreamToString(response.Body);

			var payload = new List<string>();

			payload.Add("Recorder::SaveResponse");
			// payload.Add($"{method} {scheme}://servername:port{path}{querystring} {protocol}");
			payload.AddRange(headers);
			payload.Add("");
			// payload.Add(body);
			// await _store.Save(payload);

			var diagnosticsString = new StringBuilder();
			payload.ForEach(line => diagnosticsString.AppendLine(line));
			_logger.LogInformation(diagnosticsString.ToString());
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