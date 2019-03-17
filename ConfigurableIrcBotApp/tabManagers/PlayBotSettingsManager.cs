using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableIrcBotApp.tabManagers
{
    public class PlayBotSettingsManager
    {
        public IDictionary<string, PlayBotAction> playBotActions { get; set; }

        MainWindow main;
        JsonFileHandler jsonFileHandler;

        public PlayBotSettingsManager(MainWindow main)
        {
            this.main = main;
            this.jsonFileHandler = new JsonFileHandler();

            this.playBotActions = jsonFileHandler.loadPlayBotActions();
        }

        public void addPlayBotAction(PlayBotAction action)
        {
            this.playBotActions.Add(action.command, action);
            jsonFileHandler.writePlayBotActions(this.playBotActions);
            if (main.playBot != null)
            {
                main.playBot.actions = this.playBotActions;
            }
        }

        public void removePlayBotAction(PlayBotAction action)
        {
            this.playBotActions.Remove(action.command);
            jsonFileHandler.writePlayBotActions(this.playBotActions);
            if (main.playBot != null)
            {
                main.playBot.actions = this.playBotActions;
            }
        }

        public void saveDataOnClose()
        {
            jsonFileHandler.writePlayBotActions(this.playBotActions);
        }

    }
}
