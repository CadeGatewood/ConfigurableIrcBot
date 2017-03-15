using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private IrcClient bot;

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameInput.Text != "" &&
                passwordInput.Text != "" &&
                channelInput.Text != ""
                )
            {
                if (bot == null || (bot != null && !bot.isRunning()))
                {
                    bot = new IrcClient(this, userNameInput.Text, passwordInput.Text, channelInput.Text);
                    bot.Start();
                }
                else if (bot.isRunning())
                {
                    MessageBox.Show("You already have a bot running, please disconnect the first before attempting a new connection");
                }
            }
            else
            {
                MessageBox.Show("Please enter the relevant information");
            }

        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            bot.sendChatMessage("bye!");
            bot.Stop();
        }

        private void motdButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setMotd(motdInput.Text);
        }

        private void streamInfoButton_Click(object sender, RoutedEventArgs e)
        {
            bot.setStreamInfo(streamInfoInput.Text);
        }

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            bot.sendChatMessage(sendMessageInput.Text);
        }

        public void write(string message)
        {
            MessageBox.Show(message);
        }
    }
}
