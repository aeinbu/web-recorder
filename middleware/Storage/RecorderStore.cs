using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Middleware.Storage
{
	public class RecorderStore : IRecorderStore
	{
		public class Options
		{
			public string RootPath { get; set; }
		}

		private readonly string _rootPath;

		public RecorderStore(IOptions<Options> options)
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
			foreach (var serializer in payload.GetSerializableParts())
			{
				var path = Path.Combine(_rootPath, $"{fileNumber}{serializer.Extension}");
				using(var targetStream = File.Open(path, FileMode.Create))
				{
					await serializer.Serialize(targetStream);
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