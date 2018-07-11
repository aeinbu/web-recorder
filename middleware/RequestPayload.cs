using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class RequestPayload : ISerializablePayload
	{
		public string Extension => ".request";

		public string Path { get; set; }
		public QueryString Querystring { get; set; }
		public string Protocol { get; set; }
		public string Scheme { get; set; }
		public string Method { get; set; }
		public IEnumerable<string> Headers { get; set; }
		public string Body { get; set; }

		IEnumerable<(Func<Stream, Task> Serialize, string Extension)> ISerializablePayload.GetSerializers()
		{
			async Task Serialize(Stream targetStream)
			{
				var payload = new List<string>();

				payload.Add($"{Method} {Scheme}://servername:port{Path}{Querystring} {Protocol}");
				payload.AddRange(Headers);
				payload.Add("");
				payload.Add(Body);

				using (var sw = new StreamWriter(targetStream))
				{
					foreach (var item in payload)
					{
						await sw.WriteLineAsync(item);
					}
				}
			};

			yield return (Serialize, Extension);
		}
	}
}