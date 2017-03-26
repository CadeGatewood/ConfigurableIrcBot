using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.Configuration;



namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        List<String> settingsKeys;
        IDictionary<string, Moderator> moderators;
        IDictionary<string, Commands> commands;

        private IrcClient bot;

        private JsonFileHandler jsonFileHandler;

        public MainWindow()
        {
            InitializeComponent();
            this.settingsKeys = new List<String>(new string[] {"ip", "port", "channel", "userName", "password"});

            foreach (string key in settingsKeys)
            {
                ((TextBox)grid.FindName(key)).Text = ConfigurationManager.AppSettings[key];
            }

            jsonFileHandler = new JsonFileHandler();

            this.moderators = jsonFileHandler.loadModerators();
            this.commands = jsonFileHandler.loadCommands();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            jsonFileHandler.writeModerators(this.moderators, "moderators.JSON");
            jsonFileHandler.writeCommands(this.commands, "commands.JSON");
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
                {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void write(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text != "" &&
                password.Text != "" &&
                channel.Text != "" &&
                ip.Text != "" &&
                port.Text != "" 
                )
            {
                if (bot == null || (bot != null && !bot.isRunning()))
                {
                    bot = new IrcClient(this, userName.Text, password.Text, channel.Text, ip.Text, Int32.Parse(port.Text));
                    bot.Start();
                    bot.setModerators(this.moderators);
                    bot.setCommands(this.commands);
                }
                else if (bot.isRunning())
                {
                    System.Windows.MessageBox.Show("You already have a bot running, please disconnect the first before attempting a new connection");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter the relevant information");
            }
            
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            bot.sendChatMessage("Goodbye!");
            bot.Stop();
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            foreach (String key in this.settingsKeys)
            {
                if (settings[key] != null)
                {
                    configFile.AppSettings.Settings[key].Value = ((TextBox)grid.FindName(key)).Text;
                    configFile.Save();
                }
                else
                {
                    configFile.AppSettings.Settings.Add(key, ((TextBox)grid.FindName(key)).Text);
                    configFile.Save();
                }
            }
        }

        private void moderatorAdd_Click(object sender, RoutedEventArgs e)
        {
            this.moderators[moderatorInput.Text] = new Moderator(moderatorInput.Text, Int32.Parse(authLevelBox.Text));
            bot.setModerators(this.moderators);
        }

        private void moderatorRemove_Click(object sender, RoutedEventArgs e)
        {
            this.moderators.Remove(moderatorInput.Text);
            bot.setModerators(this.moderators);
        }

        private void motdButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setMotd(motdInput.Text);
        }

        private void clearMotdButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setMotd("");
        }

        private void streamInfoButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setStreamInfo(streamInfoInput.Text);
        }

        private void clearStreamInfoButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setStreamInfo("");
        }

        private void commandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands["!"+commandInput.Text] = new Commands(commandInput.Text, responseInput.Text, Int32.Parse(authInput.Text));
            bot.setCommands(this.commands);
        }

        private void clearCommandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands.Remove("!"+commandInput.Text);
            bot.setCommands(this.commands);
        }
    }
}
