using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp
{
    public class PlayBot
    {
        public IDictionary<string, PlayBotAction> actions { get; set; }
        public string emulationProcess { get; set; }
        public void playBotSetup()
        {

        }

    }
}
