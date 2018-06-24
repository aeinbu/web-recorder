using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	//TODO: rename to IRecorderStore
	public interface IRequestStore
	{
		Task<int> Save(ISerializablePayload payload);
		Task Save(ISerializablePayload payload, int filenumber);
	}
}