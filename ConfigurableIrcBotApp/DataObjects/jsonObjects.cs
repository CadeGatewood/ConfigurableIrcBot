using System;
using System.Windows.Forms;

namespace ConfigurableIrcBotApp
{
    public class Moderator
    {

        public string userName { get; set; }
        public int authLevel { get; set; }

        public Moderator(string userName, int authLevel)
        {
            this.userName = userName;
            this.authLevel = authLevel;
        }

    }

    public class Commands
    {
        public string command { get; set; }
        public string response { get; set; }
        public int requiredAuthLevel { get; set; } 

        public Commands(string command, string response, int requiredAuthLevel)
        {
            this.command = command;
            this.response = response;
            this.requiredAuthLevel = requiredAuthLevel;
        }
    }

    public class PlayBotAction
    {
        public string command { get; set; }
        public string output { get; set; }
        public string description { get; set; }

        public Keys keyPress { get; set; }
        public TimeSpan duration { get; set; }
    }
}
