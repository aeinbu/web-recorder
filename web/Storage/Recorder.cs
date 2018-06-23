using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace web.Storage
{
	class Recorder
	{
		private readonly Store _store;

		public Recorder(Store Store)
		{
			_store = Store;
		}

		public async Task Handle(HttpRequest request)
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

		private async Task<string> ResolveStreamToString(Stream stream)
		{
			using (var sr = new StreamReader(stream))
			{
				return await sr.ReadToEndAsync();
			}
		}

	}
}