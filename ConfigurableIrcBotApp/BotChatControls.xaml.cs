using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for BotChatControls.xaml
    /// </summary>
    public partial class BotChatControls : Window
    {

        public IDictionary<string, Moderator> moderators { get; set; }
        public IDictionary<string, Commands> commands { get; set; }

        private MainWindow main;
        private JsonFileHandler jsonFileHandler;
        public IrcClient bot { get; set; }

        public BotChatControls(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            this.jsonFileHandler = main.jsonFileHandler;
            this.bot = main.bot;
            this.moderators = main.moderators;
            this.commands = main.commands;
        }

        private void chatBotControl_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void moderatorAdd_Click(object sender, RoutedEventArgs e)
        {
            this.moderators[moderatorInput.Text] = new Moderator(moderatorInput.Text, Int32.Parse(authLevelBox.Text));
            jsonFileHandler.writeModerators(moderators);
            bot.setModerators(this.moderators);
        }

        private void moderatorRemove_Click(object sender, RoutedEventArgs e)
        {
            this.moderators.Remove(moderatorInput.Text);
            jsonFileHandler.writeModerators(moderators);
            bot.setModerators(this.moderators);
        }

        private void commandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands["!" + commandInput.Text] = new Commands(commandInput.Text, responseInput.Text, Int32.Parse(authInput.Text));
            jsonFileHandler.writeCommands(commands);
            bot.setCommands(this.commands);
        }

        private void clearCommandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands.Remove("!" + commandInput.Text);
            jsonFileHandler.writeCommands(commands);
            bot.setCommands(this.commands);
        }

    }
}
