using System;
using System.IO;
using System.Threading.Tasks;

namespace Middleware
{
	public class SerializableBinaryPayloadPart : ISerializablePayloadPart
	{
		public string Extension { get; set; }
		private Stream _payload;

		public SerializableBinaryPayloadPart(string extension, Stream payload)
		{
			Extension = extension;
			_payload = payload;
		}

		public SerializableBinaryPayloadPart(string extension, byte[] payload)
			: this(extension, new MemoryStream(payload))
		{
		}

		public async Task Serialize(Stream target)
		{
			_payload.Seek(0, SeekOrigin.Begin);
			await _payload.CopyToAsync(target);
		}
	}
}