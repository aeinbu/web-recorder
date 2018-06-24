using System.Collections.Generic;

namespace Middleware
{
	public class ResponsePayload : ISerializablePayload
	{
		public string Extension => ".response";

		public string Protocol { get; set; }
		public int StatusCode { get; set; }
		public object ReasonPhrase { get; set; }
		public IEnumerable<string> Headers { get; set; }
		public string Body { get; set; }

		IEnumerable<string> ISerializablePayload.Serialize() => Serialize();
		public List<string> Serialize()
		{
			//TODO:
			// -- sample response source --
			// HTTP/1.1 200 OK
			// Date: Sun, 24 Jun 2018 12:34:54 GMT
			// Server: Kestrel
			// Transfer-Encoding: chunked

			var payload = new List<string>();
			payload.Add($"{Protocol} {StatusCode} {ReasonPhrase}");
			payload.AddRange(Headers);
			payload.Add("");
			payload.Add(Body);

			return payload;
		}
	}
}