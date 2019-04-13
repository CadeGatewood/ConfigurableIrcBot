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
        public List<string> commandNames = new List<string> { "!firstinfirstout", "!allatonce" };

        public ComplexCommands(MainWindow main)
        {
            this.main = main;
        }
        
        public void processComplexCommand(string command)
        {
            switch (command.ToLower())
            {
                case "!firstinfirstout":
                    processVote(1);
                    break;
                case "!allatonce":
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