using System.IO;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	//TODO: rename to IPlayerStore
	public interface IResponseStore
	{
		Task<Stream> Load(string method, string path);
	}
}