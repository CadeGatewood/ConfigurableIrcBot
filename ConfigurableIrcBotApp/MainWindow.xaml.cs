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

using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        List<String> settingsKeys;
        IDictionary<string, Moderator> moderators;

        string moderatorsFile = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "moderators.JSON";

        public MainWindow()
        {
            InitializeComponent();
            this.settingsKeys = new List<String>(new string[] {"ip", "port", "channel", "userName", "password"});

            foreach (string key in settingsKeys)
            {
                ((TextBox)grid.FindName(key)).Text = ConfigurationManager.AppSettings[key];
            }



        }



        private void portInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private IrcClient bot;

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

        private void loadModerators()
        {
            JsonTextReader jsonReader;
            StreamReader fileRead;

            try
            {
                fileRead = File.OpenText(moderatorsFile);
                jsonReader = new JsonTextReader(fileRead);
            }
            catch (FileNotFoundException)
            {
                File.Create(moderatorsFile);
                fileRead = File.OpenText(moderatorsFile);
                jsonReader = new JsonTextReader(fileRead);
            }

            JsonSerializer serializer = new JsonSerializer();
            List<Moderator> savedModerators = (List<Moderator>)serializer.Deserialize(jsonReader, typeof(Moderator));
            foreach (Moderator mod in savedModerators)
            {
                this.moderators[mod.userName] = mod;
            }

        }

        private void moderatorAdd_Click(object sender, RoutedEventArgs e)
        {
            loadModerators();

            this.moderators.Add(moderatorInput.Text, new Moderator(moderatorInput.Text, 0));
        }

        private void moderatorRemove_Click(object sender, RoutedEventArgs e)
        {
            string fileLocation = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "moderators.JSON";

            loadModerators();

            this.moderators.Remove(moderatorInput.Text);

        }

        private void writeModeratorsFile()
        {
            File.WriteAllText(moderatorsFile, JsonConvert.SerializeObject(moderators));
        }
    }
}
