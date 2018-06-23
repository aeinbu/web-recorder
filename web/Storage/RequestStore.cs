using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Storage
{

	class RequestStore : IRequestStore
	{
		private const string requestExtension = ".request";
		private readonly string _rootpath;

		public RequestStore(string rootpath)
		{
			_rootpath = rootpath;
		}

		public async Task Save(IEnumerable<string> payload)
		{
			var nextFileNumber = GetHighestFileNumber() + 1 ?? 0;
			var filename = Path.Combine(_rootpath, $"{nextFileNumber}{requestExtension}");

			using (var sw = new StreamWriter(filename))
			{
				foreach (var item in payload)
				{
					await sw.WriteLineAsync(item);
				}
			}
		}

		private int? GetHighestFileNumber()
		{
			var files = Directory.EnumerateFiles(_rootpath)
								.Where(file => file.EndsWith(requestExtension))
								.Select(file => Path.GetFileName(file))
								.Select(file => file.Substring(0, file.Length - requestExtension.Length));

			if (!files.Any(file => int.TryParse(file, out _)))
			{
				return null;
			}

			return files.Select(file => int.Parse(file)).Max();
		}
	}
}