using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Storage
{
	interface IRequestStore
	{
		Task Save(IEnumerable<string> payload);
	}
}