using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchConfig
{
	class Texts
	{
		public static string Title =
			  "SwitchConfig by Wolfsblvt" + Environment.NewLine
			+ "=========================" + Environment.NewLine;
		public static string Tab = "\t";

		public static string Choose = "Choose config file:";
		public static string ChooseHelp = "You can press TAB to switch between possible files";
		public static string Done = "Config file successfully written.";

		public static string NoConfigFile = "There is currently no config file specified.";
		public static string CurrentConfigFile = "Current config: {0}";
		public static string WarningConfigNotSaved = "WARNING: Your current config file is not saved. If you replace it, it will be lost. Do you want to save it? (Press 'Y' for yes and 'N' for no)";
		public static string SpecifyNewName = "Specify name to save the config:";
		public static string SavedCurrentConfig = "Current config saved as {0}";

		public static string ErrorForceWithoutFile = "ERROR: When you use '--force', you need to specify a file with '--file filename'.";
		public static string ErrorCouldNotWriteFile = "ERROR: Could not write file.";
		public static string ErrorFileNotFound = "ERROR: File {0} not found.";
		public static string ErrorNoFiles = "ERROR: No config files found."
			+ Environment.NewLine
			+ @"You can specify another path then the current folder by adding '--path the\path\to\your\repo\' as a parameter.";

		public static string WarningInvalidArgs = "WARNING: Invalid arguments entered.";

		public static string Helpline =
			  "Following Parameters are allowed:" + Environment.NewLine
			+ Texts.Tab + "--help" + Texts.Tab + "Displays this help" + Environment.NewLine
			+ Texts.Tab + "--dir ____" + Texts.Tab + "Specify the path to the directory of phpBB" + Environment.NewLine
			+ Texts.Tab + "--file ____" + Texts.Tab + "Specify the filename wich should be used as config" + Environment.NewLine
			+ Texts.Tab + "--force" + Texts.Tab + "Force execution without interruption" + Environment.NewLine;
	}
}
