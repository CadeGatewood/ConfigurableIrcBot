using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp.DataObjects
{
    public class ComplexCommands
    {
        MainWindow main;
        public List<string> commandNames = new List<string> { "!FirstInFirstOut", "!AllAtOnce" };

        public ComplexCommands(MainWindow main)
        {
            this.main = main;
        }
        
        public void processComplexCommand(string command)
        {
            switch (command)
            {
                case "!FirstInFirstOut":
                    processVote(1);
                    break;
                case "!AllAtOnce":
                    processVote(-1);
                    break;
            }
        }

        public void processVote(int vote)
        {
            if (main.voteResults + vote > 100 || main.voteResults + vote < 1)
                return;

            main.voteResults += vote;
            main.popOutChat.updateVote();
        }

    }
}
