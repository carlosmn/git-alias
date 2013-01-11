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
			var aliases = cfg.OfType<ConfigurationEntry<string>>()
				.Where(e => e.Key.StartsWith(prefix))
					.Select(e => new { Name = e.Key.Substring(prefix.Length), Value = e.Value }); 

			// make it prettier and align the equal sign
			var width = aliases.Select(p => p.Name.Length).Max();
			foreach (var alias in aliases)
				Console.WriteLine("{0} = {1}", alias.Name.PadRight(width), alias.Value);
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
			using (var cfg = new Configuration())
			{
				switch (args.Length) {
				case 0:
					List(cfg);
					break;
				case 1:
					ListOne(cfg, args [0]);
					break;
				case 2:
					Set(cfg, args [0], args [1]);
					break;
				}
			}
		}
	}
}
