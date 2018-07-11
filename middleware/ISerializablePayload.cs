using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Middleware
{
	public interface ISerializablePayload
	{
		IEnumerable<(Func<Stream, Task> Serialize, string Extension)> GetSerializers();
	}
}