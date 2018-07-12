using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Middleware
{
	public class SerializableTextPayloadPart : ISerializablePayloadPart
	{
		public string Extension { get; }
		private IEnumerable<string> _payload;

		public SerializableTextPayloadPart(string extension, IEnumerable<string> payload)
		{
			Extension = extension;
			_payload = payload;
		}

		public SerializableTextPayloadPart(string extension, string payload)
			: this(extension, new[] { payload })
		{
		}

		public async Task Serialize(Stream target)
		{
			using (var sw = new StreamWriter(target))
			{
				var count = 0;
				foreach (var item in _payload)
				{
					if (count++ > 0)
					{
						// newline after all items, except the last one...
						await sw.WriteLineAsync();
					}
					await sw.WriteAsync(item);
				}
			}
		}
	}
}