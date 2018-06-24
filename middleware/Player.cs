using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Storage;

namespace Middleware
{
	public class Player
	{
		private readonly IResponseStore _storage;
		private readonly ILogger _logger;

		public Player(IResponseStore Storage, ILogger<Player> logger)
		{
			_storage = Storage;
			_logger = logger;
		}

		public async Task<Stream> Handle(HttpRequest request)
		{
			return await _storage.Load(request.Method, request.Path);
		}
	}
}