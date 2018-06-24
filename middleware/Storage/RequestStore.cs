using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Middleware.Storage
{
	public class RequestStore : IRequestStore
	{
		public class Options
		{
			public string RootPath { get; set; }
		}

		private readonly string _rootPath;

		public RequestStore(IOptions<Options> options)
		{
			_rootPath = options.Value.RootPath;
		}

		public async Task<int> Save(ISerializablePayload payload)
		{
			var nextFileNumber = GetHighestFileNumber() + 1 ?? 0;
			await Save(payload, nextFileNumber);
			return nextFileNumber;
		}

		public async Task Save(ISerializablePayload payload, int fileNumber)
		{
			var filename = Path.Combine(_rootPath, $"{fileNumber}{payload.Extension}");
			using (var sw = new StreamWriter(filename))
			{
				foreach (var item in payload.Serialize())
				{
					await sw.WriteLineAsync(item);
				}
			}
		}

		private static object _getFileNumberLock = new Object();

		private int? GetHighestFileNumber()
		{
			lock (_getFileNumberLock)
			{
				var files = Directory.EnumerateFiles(_rootPath)
									.Select(file => Path.GetFileName(file))
									.Where(file => file.IndexOf('.') > 0)
									.Select(file => file.Substring(0, file.IndexOf('.')));

				if (!files.Any(file => int.TryParse(file, out _)))
				{
					return null;
				}

				return files.Select(file => int.Parse(file)).Max();
			}
		}
	}
}