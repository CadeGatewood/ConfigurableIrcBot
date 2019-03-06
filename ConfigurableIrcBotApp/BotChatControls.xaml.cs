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

        IDictionary<string, Moderator> moderators;

        MainWindow main;
        IrcClient bot;

        public BotChatControls(MainWindow main, IrcClient bot)
        {
            InitializeComponent();
            this.main = main;
            this.bot = bot;
            this.moderators = main.getModerators();
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
    }
}
