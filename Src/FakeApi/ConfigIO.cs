using System;
using System.IO;
using Newtonsoft.Json;

namespace FakeApi
{
    internal static class ConfigIO
    {
        public static Config GetConfig(string configSource)
        {
            if(!File.Exists(configSource))
            {
                throw new FileLoadException($"File {configSource} not found");
            }

            var configText = File.ReadAllText(configSource);

            Config config;
            try
            {
                config = JsonConvert.DeserializeObject<Config>(configText);
            }
            catch (JsonReaderException)
            {
                throw;
            }

            if(config == null)
            {
                throw new FileLoadException($"An error occured when deserialized file content");
            }

            config.Validate();

            return config;
        }

        public static void WriteConfig(Config config, string configFilePath)
        {
            if(config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if(File.Exists(configFilePath))
            {
                File.Delete(configFilePath);
            }

            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config));
        }
    }
}
