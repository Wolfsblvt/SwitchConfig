# SwitchConfig
Allows to switch between different config.php files in a dev environment.

![SwitchConfig Console Output](http://i.imgur.com/TfShiWg.png)

## Installation
To install this tool, you need to build the project and use the generated `SwitchConfig.exe`.
Otherwise you can take the latest exe from https://github.com/Wolfsblvt/SwitchConfig/releases

You should use the executable in your phpBB root path (total root or inside "phpBB" doesn't matter), or you have to specify the correct directory with `--dir` everytime you run the tool.
To run it, you can simply click on the file and it will open and ask what you want to do, or you can call it via command line and add parameters

## Help
Following Parameters are allowed

| Parameter | Help Text |
| --------- | --------- |
| --help | Displays this help |
| --dir `your/dir` | Specify the path to the directory of phpBB |
| --file `config_name.php` | Specify the filename wich should be used as config |
| --force | Force execution without interruption |
| --show | Shows the current used config |

All files you want to use as a config must be named `config_****.php`. (Where `****` can be any numbers or letters)
You can switch between possible config by pressing TAB if you are asked to enter your config name.

## Possible Uses Cases
1. **I want to change my config**<br />
=> `SwitchConfig`
2. **I want to change the used config everytime I start debugging**<br />
=> `SwitchConfig --file config_dev.php --force`
3. **I want to see wich of the config files I am using at the moment**<br />
=> `SwitchConfig --show`
