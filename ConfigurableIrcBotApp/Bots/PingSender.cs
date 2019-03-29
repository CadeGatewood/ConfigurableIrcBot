using ConfigurableIrcBotApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConfigurableIrcBotApp
{
    /*
    * Class that sends PING to irc server every 5 minutes
    */
    public class PingSender
    {
        IrcClient ircClient;

        static string PING = "PING ";
        private Thread pingSender;

        // Empty constructor makes instance of Thread
        public PingSender(IrcClient ircClient)
        {
            this.ircClient = ircClient;
            pingSender = new Thread(new ThreadStart(this.Run));
        }
        // Starts the thread
        public void Start()
        {
            pingSender.Start();
        }
        // Send PING to irc server every 5 minutes
        public void Run()
        {
            while (true)
            {
                ircClient.sendIrcMessage(PING + "irc.twitch.tv");
                Thread.Sleep(300000); // 5 minutes
            }
        }
    }
}