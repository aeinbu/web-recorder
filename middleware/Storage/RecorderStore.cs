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
		private static int? _nextFreeFileNumber = null;	//NOTE: this is a shared resource. Remember to lock when manipulating
		private static object _getNextFreeFileNumberLock = new Object();

		public RecorderStore(IOptions<Options> options)
		{
			_rootPath = options.Value.RootPath;
		}

		public async Task<int> Save(ISerializablePayload payload)
		{
			var nextFileNumber = GetNewFileNumber();
			await Save(payload, nextFileNumber);
			return nextFileNumber;
		}

		public async Task Save(ISerializablePayload payload, int fileNumber)
		{
			foreach (var serializer in payload.GetSerializableParts())
			{
				var path = Path.Combine(_rootPath, $"{fileNumber}{serializer.Extension}");
				using (var targetStream = File.Open(path, FileMode.Create))
				{
					await serializer.Serialize(targetStream);
				}
			}
		}

		private int GetNewFileNumber()
		{
			lock (_getNextFreeFileNumberLock)
			{
				if (!_nextFreeFileNumber.HasValue)
				{
					_nextFreeFileNumber = GetHighestUsedFileNumber() ?? 0;
				}

				return (int)_nextFreeFileNumber++;
			}
		}

		private int? GetHighestUsedFileNumber()
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