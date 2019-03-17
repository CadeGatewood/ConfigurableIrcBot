using ConfigurableIrcBotApp.tabManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConfigurableIrcBotApp
{
    public partial class EditCommands : Window
    {
        MainWindow main { get; set; }
        BotChatActivitySettingsManager botChatActivityManager;
        List<Commands> commandsList;
        public EditCommands(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            this.botChatActivityManager = main.botChatActivity;

            this.commandsList = botChatActivityManager.commands.Values.ToList();
            commandsGrid.ItemsSource = commandsList;
        }

        private void editCommands_Closing(object sender, CancelEventArgs e)
        {
                e.Cancel = true;
                Hide();   
        }
        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Commands newCommand = new Commands(main.editCommands.commadnInput.Text, main.editCommands.responseInput.Text, Int32.Parse(main.editCommands.authorizationInput.Text));
                botChatActivityManager.addComand(newCommand);
                commandsList.Add(newCommand);
                commandsGrid.Items.Refresh();
            }
            catch(Exception newCommandException)
            {
                main.write("Please enter all information for a command");
            }
        }

        private void removeCommand_Click(object sender, RoutedEventArgs e)
        {
            var command = (Commands)commandsGrid.SelectedItem;
            botChatActivityManager.removeCommand(command);
            commandsList.Remove(command);
            this.commandsGrid.Items.Refresh();
        }
    }
}
