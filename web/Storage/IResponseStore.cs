using System.IO;
using System.Threading.Tasks;

namespace Web.Storage
{
	interface IResponseStore
	{
		Task<Stream> Load(string method, string path);
	}
}