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
		public Stream Body { get; set; }

		IEnumerable<ISerializablePayloadPart> ISerializablePayload.GetSerializableParts()
		{
			var responsePayload = new List<string>();
			responsePayload.Add($"{Protocol} {StatusCode} {ReasonPhrase}");
			responsePayload.AddRange(Headers);
			yield return new SerializableTextPayloadPart(ResponseExtension, responsePayload);

			using (var ms = new MemoryStream())
			{
				Body.CopyTo(ms);
				if (ms.Length > 0)
				{
					yield return new SerializableBinaryPayloadPart(ResponseBodyExtension, ms);
				}
			}
		}
	}
}