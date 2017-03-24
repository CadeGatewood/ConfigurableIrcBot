using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ConfigurableIrcBotApp
{
    class ModeratorJson
    {
        string moderatorsFile = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "moderators.JSON";

        public IDictionary<string, Moderator> loadModerators()
        {
            JsonTextReader jsonReader;
            StreamReader fileRead;

            IDictionary<string, Moderator> result = new Dictionary<string, Moderator>();

            try
            {
                fileRead = File.OpenText(moderatorsFile);
                jsonReader = new JsonTextReader(fileRead);
            }
            catch (FileNotFoundException)
            {
                File.Create(moderatorsFile);
                fileRead = File.OpenText(moderatorsFile);
                jsonReader = new JsonTextReader(fileRead);
            }

            JsonSerializer serializer = new JsonSerializer();
            IDictionary<string, Moderator> savedModerators = (Dictionary<string, Moderator>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, Moderator>));
            if (savedModerators != null)
            {
                foreach (string mod in savedModerators.Keys)
                {
                    result[mod] = savedModerators[mod];
                }
            }
            fileRead.Close();
            jsonReader.Close();

            return result;
        }

        public void writeModeratorsFile(IDictionary<string, Moderator> moderators)
        {
            File.WriteAllText(moderatorsFile, JsonConvert.SerializeObject(moderators, Formatting.Indented));
        }
    }
}
