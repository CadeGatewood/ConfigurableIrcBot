using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp
{
    public class Moderator
    {

        public string userName;
        public int authLevel;

        public Moderator(string userName, int authLevel)
        {
            this.userName = userName;
            this.authLevel = authLevel;
        }

    }

    public class Commands
    {
        public string command;
        public string response;
        public int requiredAuthLevel;

        public Commands(string command, string response, int requiredAuthLevel)
        {
            this.command = command;
            this.response = response;
            this.requiredAuthLevel = requiredAuthLevel;
        }
    }
}
