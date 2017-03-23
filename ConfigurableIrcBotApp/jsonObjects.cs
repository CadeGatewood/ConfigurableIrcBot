using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp
{
    class Moderator
    {

        public string userName;
        public int authLevel;

        public Moderator(string userName, int authLevel)
        {
            this.userName = userName;
            this.authLevel = authLevel;
        }

    }
}
