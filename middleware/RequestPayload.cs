using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
	public class RequestPayload : ISerializablePayload
	{
		public string RequestExtension => ".request";
		public string RequestBodyExtension => ".request.body";

		public string Path { get; set; }
		public QueryString Querystring { get; set; }
		public string Protocol { get; set; }
		public string Scheme { get; set; }
		public string Method { get; set; }
		public IEnumerable<string> Headers { get; set; }
		public string Body { get; set; }

		IEnumerable<ISerializablePayloadPart> ISerializablePayload.GetSerializableParts()
		{
			var payload = new List<string>();
			payload.Add($"{Method} {Scheme}://servername:port{Path}{Querystring} {Protocol}");
			payload.AddRange(Headers);
			// payload.Add("");
			// payload.Add(Body);

			yield return new SerializableTextPayloadPart(RequestExtension, payload);
			if (!string.IsNullOrEmpty(Body))
			{
				yield return new SerializableTextPayloadPart(RequestBodyExtension, Body);
				// yield return new SerializableBinaryPayloadPart(RequestBodyExtension, Body);
			}
		}
	}
}