using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for CommandEditor.xaml
    /// </summary>
    public partial class CommandEditor : Window
    {
        private IrcClient bot;
        private MainWindow main;

        IDictionary<string, Commands> commands;

        public CommandEditor(MainWindow main, IrcClient bot)
        {
            InitializeComponent();

            this.main = main;
            this.bot = bot;

            this.commands = main.getCommands();
        }

        private void commandEditor_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
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

        private void responseInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
