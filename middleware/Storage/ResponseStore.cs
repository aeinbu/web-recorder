using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Middleware.Storage
{

	public class ResponseStore : IResponseStore
	{
		public class Options
		{
			public string RootPath { get; set; }
		}

		private readonly string _rootpath;

		public ResponseStore(IOptions<Options> options)
		{
			_rootpath = options.Value.RootPath;
		}

		public Task<Stream> Load(string method, string resourcePath)
		{
			string filename = Path.Combine(_rootpath, Path.GetFileName(resourcePath));

			return Task.FromResult<Stream>(File.OpenRead(filename));
		}
	}
}