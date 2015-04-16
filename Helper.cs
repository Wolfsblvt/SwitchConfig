using System;
using System.Collections.Generic;
using System.Linq;

namespace SwitchConfig
{
	public static class Helper
	{
		/// <summary>
		///		Remove the last char from console.
		/// </summary>
		public static void ConsoleRemoveLastChar()
		{
			ConsoleRemoveChars(1);
		}

		/// <summary>
		///		Remove the last X chars from console.
		/// </summary>
		/// <param name="count">The number of chars to remove</param>
		public static void ConsoleRemoveChars(int count)
		{
			var backsteps = new string('\b', count);

			Console.Write(backsteps);
			Console.Write(new string(' ', count));
			Console.Write(backsteps);
		}

		/// <summary>
		///		Validate the given parameters
		/// </summary>
		/// <param name="args">arguments from command line</param>
		/// <returns>Dictionary of arguments</returns>
		public static Dictionary<string, string> ValidateParameters(string[] args)
		{
			var dict = new Dictionary<string, string>(args.Length / 2);
			var invalidArguments = new List<string>(args.Length);

			string[] allowedSingleArgs = {
				"--" + Args.HELP,
				"--" + Args.FORCE,
				"--" + Args.SHOW
			};

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("--") && i + 1 < args.Length && !args[i + 1].StartsWith("--"))
				{
					dict.Add(args[i].Substring(2), args[i + 1]);
					i++;
				}
				else if (args[i].StartsWith("--") && allowedSingleArgs.Contains(args[i]))
				{
					dict.Add(args[i].Substring(2), "");
				}
				else if (i == 0 && !args[i].StartsWith("--"))
				{
					dict.Add(Args.FILE, args[i]);
				}
				else
				{
					invalidArguments.Add(args[i]);
				}
			}

			if (invalidArguments.Count > 0)
			{
				Console.WriteLine(Texts.WarningInvalidArgs);
				Console.WriteLine(Texts.Tab + string.Join(Environment.NewLine + Texts.Tab, invalidArguments));
			}

			return dict;
		}
	}

	public class Args
	{
		public const string HELP = "help";
		public const string DIR = "dir";
		public const string FILE = "file";

		public const string FORCE = "force";
		public const string SHOW = "show";
	}
}
