using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Middleware.Storage
{

	public class PlayerStore : IPlayerStore
	{
		public class Options
		{
			public string RootPath { get; set; }
		}

		private readonly string _rootPath;

		public PlayerStore(IOptions<Options> options)
		{
			_rootPath = options.Value.RootPath;
		}

		public Task<Stream> Load(string method, string resourcePath)
		{
			string filename = Path.Combine(_rootPath, Path.GetFileName(resourcePath));

			return File.Exists(filename)
					? Task.FromResult<Stream>(File.OpenRead(filename))
					: Task.FromResult<Stream>(null);
		}
	}
}