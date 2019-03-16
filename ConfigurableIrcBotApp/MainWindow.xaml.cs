using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;

namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        public List<String> settingsKeys { get; set; }
        public IDictionary<string, Moderator> moderators { get; set; }
        public IDictionary<string, Commands> commands { get; set; }

        public IrcClient bot { get; set; }
        public PlayBot playBot { get; set; }
        public JsonFileHandler jsonFileHandler { get; set; }

        //Windows
        public PopOutChat popOutChat { get; set; }
        public ConnectionSetup connectionSetup { get; set; }
        public BotChatControls botChatControls { get; set; }
        public ChatDisplaySettings chatDisplaySettings { get; set; }
        public bool chatPoppedOut { get; set; }

        public MainWindow(ConnectionSetup connectionSetup, IrcClient bot, List<String> settingsKeys)
        {
            InitializeComponent();
            
            buildFileStructure();
            
            jsonFileHandler = new JsonFileHandler();

            this.moderators = jsonFileHandler.loadModerators();
            if (moderators == null)
                moderators = new Dictionary<string, Moderator>();
            this.commands = jsonFileHandler.loadCommands();
            if (commands == null)
                commands = new Dictionary<string, Commands>();

            this.connectionSetup = connectionSetup;
            this.bot = bot;

            this.popOutChat = new PopOutChat(this);
            this.botChatControls = new BotChatControls(this);
            this.chatDisplaySettings = new ChatDisplaySettings(this);         

            this.settingsKeys = settingsKeys;
            
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            jsonFileHandler.writeModerators(this.moderators);
            jsonFileHandler.writeCommands(this.commands);
            System.Windows.Application.Current.Shutdown();
        }

        private void buildFileStructure()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Fonts");
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\SavedConfigurations");
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
            botChatControls.bot = bot;
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

        private void channelConfig_Click(object sender, RoutedEventArgs e)
        {
            botChatControls.Show();
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

        private void popOutChatConfig_Click(object sender, RoutedEventArgs e)
        {
            chatDisplaySettings.Show();
        }
    }
}
