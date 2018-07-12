using System.Collections.Generic;

namespace Middleware
{

	public interface ISerializablePayload
	{
		IEnumerable<ISerializablePayloadPart> GetSerializableParts();
	}
}