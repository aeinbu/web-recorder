using System.Collections.Generic;

namespace Middleware
{
	public interface ISerializablePayload
	{
		string Extension { get; }
		IEnumerable<string> Serialize();
	}
}