using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp.tabManagers
{
    public class BotChatActivitySettingsManager
    {

        public IDictionary<string, Moderator> moderators { get; set; }
        public IDictionary<string, Commands> commands { get; set; }


        private MainWindow main;
        private JsonFileHandler jsonFileHandler;

        private IrcClient bot;

        public BotChatActivitySettingsManager(MainWindow main)
        {
            this.main = main;
            this.jsonFileHandler = new JsonFileHandler();
            this.bot = main.bot;

            this.moderators = jsonFileHandler.loadModerators();
            this.commands = jsonFileHandler.loadCommands();
        }

        public void setupBot(IrcClient bot)
        {
            this.bot = bot;
        }

        public void addModerator(Moderator moderator)
        {
            this.moderators[moderator.userName] = moderator;
            jsonFileHandler.writeModerators(moderators);
            bot.moderators = moderators;
        }

        public void removeModerator(Moderator moderator)
        {
            this.moderators.Remove(moderator.userName);
            jsonFileHandler.writeModerators(moderators);
            bot.moderators = moderators;
        }

        public void addComand(Commands command)
        {
            this.commands[command.command] = command;
            jsonFileHandler.writeCommands(commands);
            bot.commands = commands;
        }

        public void removeCommand(Commands command)
        {
            this.commands.Remove(command.command);
            jsonFileHandler.writeCommands(commands);
            bot.commands = commands;
        }

        public void saveDataOnClose()
        {
            jsonFileHandler.writeModerators(this.moderators);
            jsonFileHandler.writeCommands(this.commands);
        }
    }
}
