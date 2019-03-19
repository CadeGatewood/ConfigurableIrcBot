using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;


namespace ConfigurableIrcBotApp
{
    public class JsonFileHandler
    {
        string filesBase = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\SavedConfigurations";
        string moderatorFile = "moderators.json";
        string commandsFile = "commands.json";
        string playBotActionFile = "playBotActions.json";

        IDictionary<string, Commands> resultCommands;
        IDictionary<string, Moderator> resultModerators;
        IDictionary<string, PlayBotAction> resultActions;

        public void loadStoredDictionary(string file, string callingType)
        {
            JsonTextReader jsonReader;
            StreamReader fileRead;

            IDictionary<string, Commands> resultingCommands = new Dictionary<string, Commands>();
            IDictionary<string, Moderator> resultingModerators = new Dictionary<string, Moderator>();
            IDictionary<string, PlayBotAction> resultingActions = new Dictionary<string, PlayBotAction>();

            try
            {
                fileRead = File.OpenText(filesBase + file);
                jsonReader = new JsonTextReader(fileRead);
            }
            catch (FileNotFoundException)
            {
                File.Create(filesBase + file).Close();
                fileRead = File.OpenText(filesBase + file);
                jsonReader = new JsonTextReader(fileRead);
            }

            JsonSerializer serializer = new JsonSerializer();

            switch (callingType)
            {
                case "command":
                    IDictionary<string, Commands> savedCommands = (Dictionary<string, Commands>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, Commands>));
                    if (savedCommands != null)
                    {
                        foreach (string mod in savedCommands.Keys)
                        {
                            resultingCommands[mod] = savedCommands[mod];
                        }
                    }
                    fileRead.Close();
                    jsonReader.Close();
                    this.resultCommands = resultingCommands;
                    break;
                case "moderator":
                    IDictionary<string, Moderator> savedModerators = (Dictionary<string, Moderator>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, Moderator>));
                    if (savedModerators != null)
                    {
                        foreach (string mod in savedModerators.Keys)
                        {
                            resultingModerators[mod] = savedModerators[mod];
                        }
                    }
                    fileRead.Close();
                    jsonReader.Close();
                    this.resultModerators = resultingModerators;
                    break;
                case "playBotAction":
                    IDictionary<string, PlayBotAction> savedPlayBotActions = (Dictionary<string, PlayBotAction>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, PlayBotAction>));
                    if (savedPlayBotActions != null)
                    {
                        foreach (string action in savedPlayBotActions.Keys)
                        {
                            resultingActions[action] = savedPlayBotActions[action];
                        }
                    }
                    fileRead.Close();
                    jsonReader.Close();
                    this.resultActions = resultingActions;
                    break;
                default:
                    break;
            }
        }

        public IDictionary<string, Moderator> loadModerators()
        {
            loadStoredDictionary(moderatorFile, "moderator");
            return resultModerators;
        }

        public IDictionary<string, Commands> loadCommands()
        {
            loadStoredDictionary(commandsFile, "command");
            return resultCommands;
        }

        public IDictionary<string, PlayBotAction> loadPlayBotActions()
        {
            loadStoredDictionary(playBotActionFile, "playBotAction");
            return resultActions;
        }

        public void writeCommands(IDictionary<string, Commands> commands)
        {
            File.WriteAllText(filesBase + commandsFile, JsonConvert.SerializeObject(commands, Formatting.Indented));
        }

        public void writeModerators(IDictionary<string, Moderator> moderators)
        {
            File.WriteAllText(filesBase + moderatorFile, JsonConvert.SerializeObject(moderators, Formatting.Indented));
        }

        public void writePlayBotActions(IDictionary<string, PlayBotAction> playBotActions)
        {
            File.WriteAllText(filesBase + playBotActionFile, JsonConvert.SerializeObject(playBotActions, Formatting.Indented));
        }
    }
}
