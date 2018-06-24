using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.Storage;

namespace Middleware
{
	public class Player
	{
		private readonly IPlayerStore _storage;
		private readonly ILogger<PlayerMiddleware> _logger;

		public Player(IPlayerStore storage, ILogger<PlayerMiddleware> logger)
		{
			_storage = storage;
			_logger = logger;
		}

		public async Task<bool> Play(HttpRequest request, HttpResponse response)
		{
			using (Stream responseStream = await _storage.Load(request.Method, request.Path))
			{
				if (responseStream == null)
				{
					return false;
				}

				responseStream.CopyTo(response.Body);
				return true;
			}
		}
	}
}