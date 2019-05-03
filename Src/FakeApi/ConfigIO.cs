using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FakeApi
{
    internal static class ConfigIO
    {
        public static Config GetConfig(string configSource)
        {
            var config = LoadConfig(configSource);
            MergeApis(config, configSource);
            config.Validate();
            return config;
        }

        public static Config LoadConfig(string configSource)
        {
            if (!File.Exists(configSource))
            {
                throw new FileLoadException($"File {configSource} not found");
            }

            Config config = null;

            using (StreamReader file = File.OpenText(configSource))
            {
                var serializer = new JsonSerializer();
                try
                {
                    config = (Config)serializer.Deserialize(file, typeof(Config));
                }
                catch (JsonReaderException)
                {
                    throw;
                }

                if (config == null)
                {
                    throw new FileLoadException($"An error occured when deserialized file {configSource}");
                }
            }

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

        private static void MergeApis(Config config, string configSource)
        {
            if (config.ApisDirectories == null)
            {
                return;
            }

            foreach (var directory in config.ApisDirectories)
            {
                if (!Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException(directory);
                }

                foreach (var file in Directory.GetFiles(directory).Except(new List<string> { configSource }))
                {
                    var apis = LoadConfig(file).Apis;
                    config.Apis = new List<ApiConfig>(config.Apis.Union(apis));
                }
            }
        }
    }
}
