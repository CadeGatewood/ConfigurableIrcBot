using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net.Sockets;

using System.Threading;

using System.Diagnostics;

namespace ConfigurableIrcBotApp
{
    class IrcClient
    {
        private string userName;
        private string channel;
        private string password;

        private string ip = "irc.twitch.tv";
        private int port = 6667;

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private Thread ircThread;
        private bool _running;

        private string motd = "placeholder";
        private string streamInfo = "placeholder";

        public IrcClient(MainWindow main, string userName, string password, string channel)
        {
            ircThread = new Thread(new ThreadStart(Run));
            ircThread.IsBackground = true;

            this.userName = userName;
            this.password = password;
            this.channel = channel;

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
                parseMessage(readMessage());    
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
            string message = inputStream.ReadLine();
            return message;
        }

        public void parseMessage(string message)
        {
            //todo proof of concept, more robust message object planned, smarter parsing planned
            Trace.WriteLine(message);
            if (message.Contains("!hello")){
                sendChatMessage("Hi there!");
            }
            else if (message.Contains("!motd"))
            {
                sendChatMessage(motd);
            }
            else if (message.Contains("!stream"))
            {
                sendChatMessage(streamInfo);
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
