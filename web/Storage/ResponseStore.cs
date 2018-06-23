using System.IO;
using System.Threading.Tasks;

namespace Web.Storage
{

	class ResponseStore : IResponseStore
	{
		private readonly string _rootpath;

		public ResponseStore(string rootpath)
		{
			_rootpath = rootpath;
		}

		public Task<Stream> Load(string method, string resourcePath)
		{
			string filename = Path.Combine(_rootpath, Path.GetFileName(resourcePath));

			return Task.FromResult<Stream>(File.OpenRead(filename));
		}
	}
}