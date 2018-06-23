using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace web.Storage
{
	class Store
	{
		private readonly string _rootpath;

		public Store(string rootpath)
		{
			_rootpath = rootpath;
		}

		public async Task Save(IEnumerable<string> payload)
		{
			var nextFileNumber = GetHighestFileNumber() + 1 ?? 0;
			var filename = Path.Combine(_rootpath, $"{nextFileNumber}.request");

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
			var files = Directory.EnumerateFiles(_rootpath);
			if (files.Count() == 0)
			{
				return null;
			}

			return files.Select(file => int.Parse(Path.GetFileNameWithoutExtension(file))).Max();
		}
	}
}