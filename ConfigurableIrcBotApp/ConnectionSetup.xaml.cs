﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for ConnectionSetup.xaml
    /// </summary>
    public partial class ConnectionSetup : Window
    {
        List<String> settingsKeys;
        IDictionary<string, Moderator> moderators { get; set; }
        IDictionary<string, Commands> commands { get; set; }

        private JsonFileHandler jsonFileHandler;

        private IrcClient bot;
        private MainWindow main;

        public ConnectionSetup()
        {
            InitializeComponent();

            this.settingsKeys = new List<String>(new string[] { "ip", "port", "channel", "userName", "password" });
            foreach (string key in settingsKeys)
            {
                ((TextBox)connectionGrid.FindName(key)).Text = ConfigurationManager.AppSettings[key];
            }
            
            jsonFileHandler = new JsonFileHandler();

            this.moderators = jsonFileHandler.loadModerators();
            if (moderators == null)
                moderators = new Dictionary<string, Moderator>();
            this.commands = jsonFileHandler.loadCommands();
            if (commands == null)
                commands = new Dictionary<string, Commands>();

            this.main = new MainWindow(this, this.bot, settingsKeys, jsonFileHandler, moderators, commands);
        }

        private void connectionSetup_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
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
                if (bot == null || (bot != null && !bot._ircrunning))
                {
                    this.bot = new IrcClient(main, userName.Text, password.Text, channel.Text, ip.Text, Int32.Parse(port.Text), this.moderators, this.commands);
                    bot.IrcStart();

                    main.botSetup(bot);
                    main.Show();
                    this.Hide();
                }
                else if (bot._ircrunning)
                {
                    System.Windows.MessageBox.Show("You already have a bot running, please disconnect the first before attempting a new connection");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter the relevant information");
            }
            
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            foreach (String key in this.settingsKeys)
            {
                if (settings[key] != null)
                {
                    configFile.AppSettings.Settings[key].Value = ((TextBox)connectionGrid.FindName(key)).Text;
                    configFile.Save();
                }
                else
                {
                    configFile.AppSettings.Settings.Add(key, ((TextBox)connectionGrid.FindName(key)).Text);
                    configFile.Save();
                }
            }
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            bot.IrcStop();
        }
    }
}
