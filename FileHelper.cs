using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwitchConfig
{
	public class FileHelper
	{
		/// <summary>
		///		The current directory where the config is searched
		/// </summary>
		public string RelevantDirectory { get; private set; }
		
		/// <summary>
		///		A list of possible subdirs for config files
		/// </summary>
		public string[] PossibleSubdirs { get; private set; }

		/// <summary>
		///		The list of found config files
		/// </summary>
		public List<string> ConfigFiles { get; private set; }

		/// <summary>
		///		Constructor
		/// </summary>
		public FileHelper() : this(Environment.CurrentDirectory)
		{
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="SearchDirectory">The fully qualified directory path</param>
		public FileHelper(string SearchDirectory)
		{
			this.RelevantDirectory = SearchDirectory;
			this.PossibleSubdirs = new string[] { "phpBB" };
		}

		/// <summary>
		///		Returns if config files were found.
		/// </summary>
		/// <returns>True if files are found, otherwise false</returns>
		public bool HasConfigFiles()
		{
			return (this.ConfigFiles.Count > 0);
		}

		/// <summary>
		///		Searches for all config files and returns the correct path for each of them
		/// </summary>
		/// <returns>True if files are found, otherwise false</returns>
		public bool FindConfigFiles()
		{
			return this.FindConfigFiles(true);
		}

		/// <summary>
		///		Searches for all config files and returns the correct path for each of them
		/// </summary>
		/// <param name="withSubdir">Bool if specified subdirectories should be searched too.</param>
		/// <returns>True if files are found, otherwise false</returns>
		public bool FindConfigFiles(bool withSubdir)
		{
			Regex reg = new Regex(@"^config(_.+)\.php");

			//List<string> files = Directory.GetFiles(this.RelevantDirectory, "*.php");
			List<string> files = Directory.GetFiles(this.RelevantDirectory, "*.php")
										.Select(x => Path.GetFileName(x))
										.Where(path => reg.IsMatch(path))
										.OrderBy(x => x).ToList();

			this.ConfigFiles = files;

			// If list is empty, check subfolders
			if (withSubdir && this.ConfigFiles.Count == 0)
			{
				var startdir = this.RelevantDirectory;
				foreach (string subdir in this.PossibleSubdirs)
				{
					this.RelevantDirectory = Directory.GetDirectories(startdir, subdir).FirstOrDefault();

					if (this.RelevantDirectory == null)
					{
						continue;
					}

					bool found = this.FindConfigFiles(false);

					if (found)
					{
						return found;
					}
				}

				// We haven't found other fitting subdirs, so set RelevantDirectory back
				this.RelevantDirectory = startdir;
			}

			return this.HasConfigFiles();
		}

		/// <summary>
		///		Check for the current chosen config file
		/// </summary>
		/// <param name="read">Should read for save file</param>
		/// <param name="justCheck">Should just check for current config name</param>
		public void CheckCurrentConfig(bool read, bool justCheck = false)
		{
			var configFile = this.FullFileName("config.php");

			if (!File.Exists(configFile))
			{
				Console.WriteLine(Texts.CurrentConfigFile, Texts.NoConfigFile);
			}
			else
			{
				// Check if current config is saved as another file
				var configFileBytes = File.ReadAllBytes(configFile);

				foreach (string file in this.ConfigFiles)
				{
					var fileBytes = File.ReadAllBytes(this.FullFileName(file));

					if (this.FileEquals(configFileBytes, fileBytes))
					{
						Console.WriteLine(Texts.CurrentConfigFile, file);
						Console.WriteLine();
						return;
					}
				}

				if (justCheck)
				{
					return;
				}

				Console.WriteLine(Texts.WarningConfigNotSaved);

				if (read)
				{
					bool correctKey = false;
					do
					{
						var key = Console.ReadKey(true);

						if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.N)
						{
							Console.WriteLine(key.KeyChar);
							correctKey = true;

							if (key.Key == ConsoleKey.Y)
							{
								Console.WriteLine(Texts.SpecifyNewName);

								string line = Texts.Tab + "-> " + "config_";
								Console.Write(line);

								// Read the filename
								string fileExt = Console.ReadLine();

								// Add the php extension if missing
								if (!fileExt.EndsWith(".php"))
								{
									fileExt += ".php";
								}
								Console.SetCursorPosition(0, Console.CursorTop - 1);
								Console.WriteLine(line + fileExt);

								// Save file
								string file = "config_" + fileExt;
								File.Copy(configFile, this.FullFileName(file));

								Console.WriteLine(Texts.SavedCurrentConfig, file);
							}
						}
					} while (!correctKey); 
				}

				Console.WriteLine();
			}
		}

		private string lastSearch { get; set; }
		private IEnumerator<string> enumerator { get; set; }

		/// <summary>
		///		Returns the next possible config file to use
		/// </summary>
		/// <param name="search">The search string for the config file</param>
		/// <returns>The next config file name</returns>
		public string GetNextConfigFile(string search)
		{
			bool next;

			if (this.enumerator != null)
			{
				next = this.enumerator.MoveNext();
				if (!next)
				{
					// Reset
					this.enumerator = this.ConfigFiles.Where(x => x.StartsWith(search)).GetEnumerator();
					this.enumerator.MoveNext();
				}

				if (string.Equals(search, this.lastSearch))
				{
					return this.enumerator.Current;
				} 
			}

			this.enumerator = this.ConfigFiles.Where(x => x.StartsWith(search)).GetEnumerator();
			next = this.enumerator.MoveNext();

			lastSearch = search;

			return (next) ? enumerator.Current : search;
		}

		/// <summary>
		///		Check if File exists
		/// </summary>
		/// <param name="file">The file name (without path)</param>
		/// <returns></returns>
		public bool FileExists(string file)
		{
			return File.Exists(this.FullFileName(file));
		}

		/// <summary>
		///		Change the given file to the used config file.
		/// </summary>
		/// <param name="file">The file name (without path)</param>
		/// <returns></returns>
		public bool ChangeFileAsConfig(string file)
		{
			var target = this.FullFileName("config.php");
			var actual = this.FullFileName(file);

			try
			{
				// Take our file and move it, replacing the current config.php
				File.Delete(target);
				File.Copy(actual, target);
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///		Returns the full file name with path.
		/// </summary>
		/// <param name="file">The file name (without path)</param>
		/// <returns>Full file name</returns>
		private string FullFileName(string file)
		{
			return Path.Combine(this.RelevantDirectory, file);
		}

		/// <summary>
		///		Compare two files byte by byte
		/// </summary>
		/// <param name="file1">First file</param>
		/// <param name="file2">Second file</param>
		/// <returns></returns>
		public bool FileEquals(byte[] file1, byte[] file2)
		{
			if (file1.Length == file2.Length)
			{
				for (int i = 0; i < file1.Length; i++)
				{
					if (file1[i] != file2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
