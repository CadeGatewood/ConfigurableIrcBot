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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConfigurableIrcBotApp
{
    /// <summary>
    /// Interaction logic for EditModerators.xaml
    /// </summary>
    public partial class EditModerators : Window
    {
        MainWindow main;
        BotChatActivitySettingsManager botChatActivity;

        List<Moderator> modsList;
        public EditModerators(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            this.botChatActivity = main.botChatActivity;

            this.modsList = botChatActivity.moderators.Values.ToList();
            moderatorGrid.ItemsSource = modsList;
        }
        private void editModerators_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void removeModerator_Click(object sender, RoutedEventArgs e)
        {
            var moderator = (Moderator)moderatorGrid.SelectedItem;
            botChatActivity.removeModerator(moderator);
            modsList.Remove(moderator);
            moderatorGrid.Items.Refresh();
        }

        private void AddModerator_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Moderator newMod = new Moderator(moderatorUserName.Text, Int32.Parse(moderatorAuthLevel.Text));
                botChatActivity.addModerator(newMod);
                modsList.Add(newMod);
                moderatorGrid.Items.Refresh();
            }
            catch (Exception newModeratorException)
            {
                main.writeError("Please enter all information for a moderator", newModeratorException);
            }
        }
    }
}
