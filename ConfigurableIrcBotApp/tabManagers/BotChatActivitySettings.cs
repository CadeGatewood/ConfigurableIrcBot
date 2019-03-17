using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp.tabManagers
{
    class BotChatActivitySettings
    {

        public IDictionary<string, Moderator> moderators { get; set; }
        public IDictionary<string, Commands> commands { get; set; }

        private MainWindow main;
        private JsonFileHandler jsonFileHandler;

        private IrcClient bot;

        public BotChatActivitySettings(MainWindow main)
        {
            this.main = main;
            this.jsonFileHandler = new JsonFileHandler();
            this.bot = main.bot;

            this.moderators = jsonFileHandler.loadModerators();
            if (moderators == null)
                moderators = new Dictionary<string, Moderator>();
            this.commands = jsonFileHandler.loadCommands();
            if (commands == null)
                commands = new Dictionary<string, Commands>();
        }

        public void addModerator()
        {
            this.moderators[main.moderatorInput.Text] = new Moderator(main.moderatorInput.Text, Int32.Parse(main.authLevelBox.Text));
            jsonFileHandler.writeModerators(moderators);
            bot.setModerators(this.moderators);
        }

        public void removeModerator()
        {
            this.moderators.Remove(main.moderatorInput.Text);
            jsonFileHandler.writeModerators(moderators);
            bot.setModerators(this.moderators);
        }

        public void addComand()
        {
            this.commands["!" + main.commandInput.Text] = new Commands(main.commandInput.Text, main.responseInput.Text, Int32.Parse(main.authInput.Text));
            jsonFileHandler.writeCommands(commands);
            bot.setCommands(this.commands);
        }

        public void clearCommand()
        {
            this.commands.Remove("!" + main.commandInput.Text);
            jsonFileHandler.writeCommands(commands);
            main.bot.setCommands(this.commands);
        }

        public void saveDataOnClose()
        {
            jsonFileHandler.writeModerators(this.moderators);
            jsonFileHandler.writeCommands(this.commands);
        }
    }
}
