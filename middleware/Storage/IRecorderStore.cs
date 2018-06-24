using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	public interface IRecorderStore
	{
		Task<int> Save(ISerializablePayload payload);
		Task Save(ISerializablePayload payload, int filenumber);
	}
}