using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Middleware
{
	public class ResponsePayload : ISerializablePayload
	{
		public string ResponseExtension => ".response";
		public string ResponseBodyExtension => ".response.body";

		public string Protocol { get; set; }
		public int StatusCode { get; set; }
		public object ReasonPhrase { get; set; }
		public IEnumerable<string> Headers { get; set; }
		public string Body { get; set; }

		IEnumerable<ISerializablePayloadPart> ISerializablePayload.GetSerializableParts()
		{
			//TODO:
			// -- sample response source --
			// HTTP/1.1 200 OK
			// Date: Sun, 24 Jun 2018 12:34:54 GMT
			// Server: Kestrel
			// Transfer-Encoding: chunked

			var responsePayload = new List<string>();
			responsePayload.Add($"{Protocol} {StatusCode} {ReasonPhrase}");
			responsePayload.AddRange(Headers);

			yield return new SerializableTextPayloadPart(ResponseExtension, responsePayload);
			if (!string.IsNullOrEmpty(Body))
			{
				yield return new SerializableTextPayloadPart(ResponseBodyExtension, Body);
				// yield return new SerializableBinaryPayloadPart(ResponseBodyExtension, Body);
			}
		}
	}
}