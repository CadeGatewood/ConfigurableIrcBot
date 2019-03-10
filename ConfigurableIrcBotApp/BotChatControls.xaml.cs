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
        public IrcClient bot { get; set; }

        public BotChatControls(MainWindow main, IrcClient bot)
        {
            InitializeComponent();
            this.main = main;
            this.bot = bot;
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
            bot.setModerators(this.moderators);
        }

        private void moderatorRemove_Click(object sender, RoutedEventArgs e)
        {
            this.moderators.Remove(moderatorInput.Text);
            bot.setModerators(this.moderators);
        }

        private void commandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands["!" + commandInput.Text] = new Commands(commandInput.Text, responseInput.Text, Int32.Parse(authInput.Text));
            bot.setCommands(this.commands);
        }

        private void clearCommandButton_Click(object sender, RoutedEventArgs e)
        {
            this.commands.Remove("!" + commandInput.Text);
            bot.setCommands(this.commands);
        }

    }
}
