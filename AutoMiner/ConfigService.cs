using System;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace AutoMiner
{
    public class ConfigService
    {
        static string CONFIG_FILE_NAME = "config.xml";
        public static AppConfig appConfig;
        static ConfigService() {
            if (File.Exists(CONFIG_FILE_NAME))
            {
                try
                {
                    appConfig = LoadConfig();
                } catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                }
            }
            if (appConfig == null)
            {
                appConfig = new AppConfig
                {
                    trexConfig = new TrexConfig
                    {
                        pool = " ",
                        execPath = " ",
                        rigName = " ",
                        wallet = " "
                    }
                };
                saveConfig();
            }
        }

        public static OcProfile getOcProfile(string gpuId)
        {
            foreach(OcProfile ocProfile in appConfig.ocProfiles)
            {
                if (ocProfile.gpuId.Equals(gpuId))
                {
                    return ocProfile;
                }
            }
            return null;
        }

        private static AppConfig LoadConfig()
        {
            using (TextReader reader = new StreamReader(CONFIG_FILE_NAME))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfig));
                return (AppConfig)xmlSerializer.Deserialize(reader);
            }          
        }

        public static void saveConfig()
        {
            using (TextWriter textWriter = new StreamWriter(CONFIG_FILE_NAME))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfig));
                xmlSerializer.Serialize(textWriter, appConfig);
            }
        }
    }

    public class AppConfig
    {
        public TrexConfig trexConfig { get; set; }
        public List<OcProfile> ocProfiles = new List<OcProfile>();
    }

    public class TrexConfig
    {
        public string execPath { get; set; }
        public string wallet { get; set; }
        public string rigName { get; set; }
        public string pool { get; set; }
    }
}
