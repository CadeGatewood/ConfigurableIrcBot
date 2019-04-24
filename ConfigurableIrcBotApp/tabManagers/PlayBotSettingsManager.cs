using System;
using System.Collections.Generic;
using System.Configuration;
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
                main.playBotActions = this.playBotActions;
            }
        }

        public void removePlayBotAction(PlayBotAction action)
        {
            this.playBotActions.Remove(action.command);
            jsonFileHandler.writePlayBotActions(this.playBotActions);
            if (main.playBot != null)
            {
                main.playBotActions = this.playBotActions;
            }
        }

        public void saveDataOnClose()
        {
            jsonFileHandler.writePlayBotActions(this.playBotActions);
        }

        public void savePlayBotSettings()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings["comboLength"] != null)
            {
                configFile.AppSettings.Settings["comboLength"].Value = main.comboLength.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("comboLength", main.comboLength.ToString());
                configFile.Save();
            }

            if (settings["repeatLength"] != null)
            {
                configFile.AppSettings.Settings["repeatLength"].Value = main.repeatLength.ToString();
                configFile.Save();
            }
            else
            {
                configFile.AppSettings.Settings.Add("repeatLength", main.repeatLength.ToString());
                configFile.Save();
            }
        }

        public void loadPlayBotSettings()
        {
            if (ConfigurationManager.AppSettings["comboLength"] != null)
            {
                main.comboLength = Int32.Parse(ConfigurationManager.AppSettings["comboLength"]);
                main.comboLengthBox.Text = main.comboLength.ToString();
            }
            if (ConfigurationManager.AppSettings["repeatLength"] != null)
            {
                main.repeatLength = Int32.Parse(ConfigurationManager.AppSettings["repeatLength"]);
                main.iterativeCommandBox.Text = main.repeatLength.ToString();
            }
        }

    }
}
