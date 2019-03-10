﻿namespace ConfigurableIrcBotApp
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
}
