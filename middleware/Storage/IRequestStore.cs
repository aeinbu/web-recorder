using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	public interface IRequestStore
	{
		Task Save(IEnumerable<string> payload);
	}
}