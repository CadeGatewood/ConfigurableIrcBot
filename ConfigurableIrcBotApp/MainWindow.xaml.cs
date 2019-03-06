using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;


namespace ConfigurableIrcBotApp
{
    public partial class MainWindow : Window
    {
        List<String> settingsKeys;
        IDictionary<string, Moderator> moderators;
        IDictionary<string, Commands> commands;

        private IrcClient bot;
        private PlayBot playBot;
        private JsonFileHandler jsonFileHandler;

        //Windows
        private PopOutChat popOutChat;
        private ConnectionSetup connectionSetup;
        private CommandEditor commandEditor;
        private BotChatControls botChatControls;


        private bool chatPoppedOut;

        public MainWindow(ConnectionSetup connectionSetup, IrcClient bot, List<String> settingsKeys, JsonFileHandler jsonFileHandler, IDictionary<string, Moderator> moderators, IDictionary<string, Commands> commands)
        {
            InitializeComponent();
            
            this.connectionSetup = connectionSetup;
            this.bot = bot;

            this.popOutChat = new PopOutChat(this);
            this.botChatControls = new BotChatControls(this, bot);
            this.commandEditor = new CommandEditor(this, bot);

            this.settingsKeys = settingsKeys;
            this.jsonFileHandler = jsonFileHandler;

            this.moderators = moderators;
            this.commands = commands;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            jsonFileHandler.writeModerators(this.moderators, "moderators.JSON");
            jsonFileHandler.writeCommands(this.commands, "commands.JSON");
            System.Windows.Application.Current.Shutdown();
        }

        public void write(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            bot.sendChatMessage("Goodbye!");
            bot.IrcStop();
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
            TextRange output = new TextRange(chatTextBox.Document.ContentEnd, chatTextBox.Document.ContentEnd) {
                Text = message.getUserName() + ": " + message.getMessage()
            };
            output.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            if (command)
            {
                output.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
            chatTextBox.AppendText(Environment.NewLine);
            chatTextBox.ScrollToEnd();
        }

        public IrcClient getCLient()
        {
            return this.bot;
        }

        public void setClient(IrcClient bot)
        {
            this.bot = bot;
        }

        private void popoutChat_Click(object sender, RoutedEventArgs e)
        {
            this.popOutChat.Show();
            chatPoppedOut = true;
        }

        public void setChatPoppedOut(bool popped)
        {
            this.chatPoppedOut = popped;
        }

        public IDictionary<string, Commands> getCommands()
        {
            return this.commands;
        }

        public IDictionary<string, Moderator> getModerators()
        {
            return this.moderators;
        }


        private void connectionConfig_Click(object sender, RoutedEventArgs e)
        {
            connectionSetup.Show();
        }

        private void channelConfig_Click(object sender, RoutedEventArgs e)
        {
            botChatControls.Show();
        }

        private void commandConfig_Click(object sender, RoutedEventArgs e)
        {
            commandEditor.Show();
        }
    }
}
