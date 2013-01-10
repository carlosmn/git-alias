using System;
using System.Linq;
using System.Collections.Generic;
using LibGit2Sharp;

namespace gitalias
{
	class MainClass
	{
		public static void List(Configuration cfg)
		{
			var aliases = new List<KeyValuePair<string,string>>();
			var prefix = "alias.";

			foreach (var entry in cfg) {
				if (entry.Key.StartsWith(prefix)) {
					var pair = new KeyValuePair<string, string>(entry.Key.Substring(prefix.Length), entry.Value);
					aliases.Add(pair);
				}
			}

			// make it prettier and align the equal sign
			var width = aliases.Select(p => p.Key.Length).Max();
			foreach (var entry in aliases)
				Console.WriteLine("{0} = {1}", entry.Key.PadRight(width), entry.Value);
		}

		public static void Main (string[] args)
		{
			var repo = new Repository(".");
			var cfg = repo.Config;

			switch (args.Length) {
			case 0:
				List(cfg);
				break;
			}
		}
	}
}
