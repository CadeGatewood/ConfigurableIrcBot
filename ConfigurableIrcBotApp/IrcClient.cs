using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net.Sockets;

using System.Threading;

namespace ConfigurableIrcBotApp
{

    public class Message
    {
        private string userName="";
        private string message="";

        public Message(){}

        public Message(string userName, string message)
        {
            this.userName = userName;
            this.message = message;
        }

        public string getMessage()
        {
            return this.message;
        }

        public string getUserName()
        {
            return this.userName;
        }
    }

    public class IrcClient
    {
        private string userName;
        private string channel;
        private string password;

        private string ip;
        private int port;

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private Thread ircThread;
        private bool _running;

        private string motd = "placeholder";
        private string streamInfo = "placeholder";

        private MainWindow main;

        IDictionary<string, Moderator> moderators;
        IDictionary<string, Commands> commands;

        public IrcClient(MainWindow main, string userName, string password, string channel, string ip, int port)
        {


            ircThread = new Thread(new ThreadStart(Run));
            ircThread.IsBackground = true;

            this.ip = ip;
            this.port = port;

            this.userName = userName;
            this.password = password;
            this.channel = channel;

            this.main = main;

        }

        public void setModerators(IDictionary<string, Moderator> moderators)
        {
            this.moderators = moderators;
        }

        public void setCommands(IDictionary<string, Commands> commands)
        {
            this.commands = commands;
        }

        public void Run()
        {
            this._running = true;

            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + userName);
            outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
            joinRoom(channel);
            outputStream.Flush();

            while (_running)
            {
                string rawMessage = inputStream.ReadLine();
                Message message = new Message();
                if (rawMessage.StartsWith(":") && rawMessage.Contains("!"))
                {
                    message = new Message(  rawMessage.Substring(rawMessage.IndexOf(":")+1, rawMessage.IndexOf("!")-1),
                                            rawMessage.Substring(rawMessage.IndexOf(":", rawMessage.IndexOf(":")+1)+1)
                                        );
                }
                parseMessage(message);
            }
        }

        public void Start()
        {
            ircThread.Start();
        }

        public void Stop()
        {
            this._running = false;
        }

        public void joinRoom(string channel)
        {
            this.channel = channel;
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
        }

        public void sendIrcMessage(String message)
        {
            outputStream.WriteLine(message);
        }

        public void sendChatMessage(string message)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
        }

        public string readMessage()
        {
            return inputStream.ReadLine();
        }

        public void parseMessage(Message message)
        {
            if (!message.getMessage().StartsWith("!")){
                main.Dispatcher.Invoke(() =>
                {
                    main.writeToChatBlock(message, false);
                });
                return;
            }



            string commandParent = message.getMessage().IndexOf(" ") > 0 ?
                    message.getMessage().Substring(0, message.getMessage().IndexOf(" ")) :
                    message.getMessage();

            main.Dispatcher.Invoke(() =>
            {
                main.writeToChatBlock(message, true);
            });

            switch (commandParent)
            {
                case "!hello":
                    sendChatMessage("Hi there, " + message.getUserName() + "!");
                    break;
                case "!motd":
                    sendChatMessage(motd);
                    break;
                case "!stream":
                    sendChatMessage(streamInfo);
                    break;
                default:
                    if (commands.ContainsKey(commandParent))
                    {
                        Commands command = commands[commandParent];
                        sendChatMessage(command.response);
                    }
                    
                    break;
            }


        }

        public Boolean isRunning()
        {
            return _running;
        }

        public void setMotd(String message)
        {
            this.motd = message;
        }

        public void setStreamInfo(string message)
        {
            this.streamInfo = message;
        }

    }
}
