using System.IO;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	public interface IPlayerStore
	{
		Task<Stream> Load(string method, string path);
	}
}