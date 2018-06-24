using System.IO;
using System.Threading.Tasks;

namespace Middleware.Storage
{
	public interface IResponseStore
	{
		Task<Stream> Load(string method, string path);
	}
}