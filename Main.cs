using System;
using System.Linq;
using System.Collections.Generic;
using LibGit2Sharp;

namespace gitalias
{
	class MainClass
	{
		static readonly string prefix = "alias.";

		public static void List(Configuration cfg)
		{
			var aliases = new List<KeyValuePair<string,string>>();

			foreach (var entry in cfg.OfType<ConfigurationEntry<string>>()) {
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

		public static void ListOne(Configuration cfg, string name)
		{
			var alias= cfg.Get<string>(String.Concat(prefix, name));
			if (alias == null)
				Console.Error.WriteLine("No such alias '{0}'", name);
			else
				Console.WriteLine("{0} = {1}", name, alias.Value);
		}

		public static void Set(Configuration allcfg, string name, string value)
		{
			allcfg.Set<string>(String.Concat(prefix, name), value, ConfigurationLevel.Global);
		}

		public static void Main (string[] args)
		{
			var repo = new Repository(".");
			var cfg = repo.Config;

			switch (args.Length) {
			case 0:
				List(cfg);
				break;
			case 1:
				ListOne(cfg, args[0]);
				break;
			case 2:
				Set(cfg, args[0], args[1]);
				break;
			}
		}
	}
}
