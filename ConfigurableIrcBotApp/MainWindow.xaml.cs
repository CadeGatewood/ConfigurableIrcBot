using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;
using ConfigurableIrcBotApp.tabManagers;
using System.Text.RegularExpressions;

namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        public List<String> settingsKeys { get; set; }
        public IDictionary<string, Moderator> moderators { get; set; }
        public IDictionary<string, Commands> commands { get; set; }
        public IDictionary<string, PlayBotAction> playBotActions { get; set; }

        public IrcClient bot { get; set; }
        public PlayBot playBot { get; set; }

        public PopoutChatSettingsManager chatSettingsManager { get; set; }
        public BotChatActivitySettingsManager botChatActivity { get; set; }
        public PlayBotSettingsManager playBotSettingsManager { get; set; }

        //Windows
        public PopOutChat popOutChat { get; set; }
        public bool chatPoppedOut { get; set; }
        public ConnectionSetup connectionSetup { get; set; }
        
        public EditCommands editCommands { get; set; }

        public MainWindow(ConnectionSetup connectionSetup, IrcClient bot, List<String> settingsKeys)
        {
            InitializeComponent();
            buildFileStructure();

            this.chatSettingsManager = new PopoutChatSettingsManager(this);
            this.botChatActivity = new BotChatActivitySettingsManager(this);
            this.playBotSettingsManager = new PlayBotSettingsManager(this);

            this.moderators = botChatActivity.moderators;
            this.commands = botChatActivity.commands;
            this.playBotActions = playBotSettingsManager.playBotActions;

            this.connectionSetup = connectionSetup;
            this.bot = bot;

            this.popOutChat = new PopOutChat(this);
            this.editCommands = new EditCommands(this);

            this.settingsKeys = settingsKeys;

            this.DragEnter += new System.Windows.DragEventHandler(fontFileDrop_DragEnter);
            this.Drop += new System.Windows.DragEventHandler(fontFileDrop_DragDrop);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            botChatActivity.saveDataOnClose();
            playBotSettingsManager.saveDataOnClose();
            System.Windows.Application.Current.Shutdown();
        }

        private void buildFileStructure()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\SavedConfigurations");
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

        private void enterSendMessage(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                sendMessage();
            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            sendMessage();
        }

        private void sendMessage()
        {
            if (bot != null)
            {
                bot.sendChatMessage(sendMessageBox.Text);
            }
            else
            {
                write("Please connect the bot to a channel before trying to send a message");
            }
        }

        public void writeToChatBlock(Message message, bool command)
        {
            if (!chatPoppedOut)
            {
                TextRange output = new TextRange(chatTextBox.Document.ContentEnd, chatTextBox.Document.ContentEnd)
                {
                    Text = message.userName + ": " + message.message + "\r"
                };
                output.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                if (command)
                {
                    output.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                }
                chatTextBox.ScrollToEnd();
            }
            else
            {
                popOutChat.writeToChat(message);
            }
        }

        public void botSetup(IrcClient bot)
        {
            this.bot = bot;
            botChatActivity.setupBot(bot);
        }

        private void ConnectionSettings_Click(object sender, RoutedEventArgs e)
        {
            connectionSetup.Show();
        }
        private void popoutChat_Click(object sender, RoutedEventArgs e)
        {
            this.popOutChat.Show();
            chatPoppedOut = true;
        }
        
        private void connectionConfig_Click(object sender, RoutedEventArgs e)
        {
            connectionSetup.Show();
        }
        
        private void start_Click(object sender, RoutedEventArgs e)
        {
            popOutChat.startTimer();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            popOutChat.pauseTimer();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            popOutChat.stopTimer();
        }

        private void changeFont_Click(object sender, RoutedEventArgs e)
        {
            this.chatSettingsManager.changeFont();
        }

        private void changeFontColor_Click(object sender, RoutedEventArgs e)
        {
            this.chatSettingsManager.changeFontColor();
        }

        public void fontFileDrop_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop)) e.Effects = System.Windows.DragDropEffects.Copy;
        }

        public void fontFileDrop_DragDrop(object sender, System.Windows.DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
        }

        private void titleChangeButton_Click(object sender, RoutedEventArgs e)
        {
            popOutChat.titleBlock.Content = titleChangeEntry.Text;
        }

        private void titleChange_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            popOutChat.titleBlock.Content = titleChangeEntry.Text;
            e.Handled = true;
        }

        private void CommndsEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.editCommands.Show();
        }
    }
}
