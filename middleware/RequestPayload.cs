using System.Collections.Generic;
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

		IEnumerable<string> ISerializablePayload.Serialize() => Serialize();
		public List<string> Serialize()
		{
			var payload = new List<string>();

			payload.Add($"{Method} {Scheme}://servername:port{Path}{Querystring} {Protocol}");
			payload.AddRange(Headers);
			payload.Add("");
			payload.Add(Body);

			return payload;
		}
	}
}