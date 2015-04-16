using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitchConfig
{
	class Program
	{
		static string RelevantDirectory;

		static void Main(string[] arguments)
		{
			RelevantDirectory = Environment.CurrentDirectory;
			Dictionary<string, string> args = Helper.ValidateParameters(arguments);

			// Write Title :P
			Console.WriteLine(Texts.Title);

			// If help, we display help
			if (args.ContainsKey(Args.HELP))
			{
				Console.WriteLine(Texts.Helpline);
				exit();
			}

			// If we have a directory specified, we use that
			if (args.ContainsKey(Args.DIR))
			{
				RelevantDirectory = args[Args.DIR];
			}

			// Get files first
			FileHelper FileHelper = new FileHelper(RelevantDirectory);
			if (!FileHelper.FindConfigFiles() && !args.ContainsKey(Args.SHOW))
			{
				Console.WriteLine(Texts.ErrorNoFiles);
				exit();
			}

			// Check current config
			FileHelper.CheckCurrentConfig(!args.ContainsKey(Args.FORCE), args.ContainsKey(Args.SHOW));

			if (args.ContainsKey(Args.SHOW))
			{
				exit();
			}

			string fileToTake;

			// Read the file we need
			if (args.ContainsKey(Args.FILE))
			{
				fileToTake = args[Args.FILE];
			}
			else
			{
				if (args.ContainsKey(Args.FORCE))
				{
					Console.WriteLine(Texts.ErrorForceWithoutFile);
					exit();
				}

				Console.WriteLine(Texts.Choose);
				Console.WriteLine("({0})", Texts.ChooseHelp);
				Console.Write(Texts.Tab + "-> ");

				List<char> entered = new List<char>(15);
				string lastFileEnd = string.Empty;
				ConsoleKeyInfo key;
				do
				{
					key = Console.ReadKey(true);

					switch (key.Key)
					{
						case ConsoleKey.Backspace:
							if (lastFileEnd != string.Empty)
							{
								lastFileEnd = lastFileEnd.Remove(lastFileEnd.Length - 1);
								Helper.ConsoleRemoveLastChar();

								entered.AddRange(lastFileEnd);
								lastFileEnd = string.Empty;
							}
							else if (entered.Count > 0)
							{
								entered.RemoveAt(entered.Count - 1);
								Helper.ConsoleRemoveLastChar();
							}

							break;
						case ConsoleKey.Tab:
							var search = string.Concat(entered);
							string file = FileHelper.GetNextConfigFile(search);

							// We need to remove things we have added with the last tab
							if (lastFileEnd != string.Empty)
							{
								Helper.ConsoleRemoveChars(lastFileEnd.Length);
							}

							lastFileEnd = file.Substring(search.Length);
							Console.Write(lastFileEnd);

							break;
						case ConsoleKey.Enter:
							Console.WriteLine();
							break;
						default:
							entered.Add(key.KeyChar);
							Console.Write(key.KeyChar);
							break;
					}
				} while (key.Key != ConsoleKey.Enter);

				fileToTake = string.Concat(entered) + lastFileEnd;
			}

			// Check if file exists
			if(!FileHelper.FileExists(fileToTake))
			{
				Console.WriteLine(Texts.ErrorFileNotFound, fileToTake);
				exit();
			}

			bool done = FileHelper.ChangeFileAsConfig(fileToTake);

			if (done)
			{
				Console.WriteLine(Texts.Done);
			}
			else
			{
				Console.WriteLine(Texts.ErrorCouldNotWriteFile);
			}
		}

		private static void exit()
		{
			Environment.Exit(0);
		}
	}
}
