using System;
using System.Collections.Generic;
using MvnoSwitcher.MobileConfig;
using Newtonsoft.Json;
using System.IO;

namespace MvnoSwitcher
{
    public class AppConfig
    {
        public List<ConfigGenerator> Apns { get; private set; }

        public AppConfig()
        {
            Apns = new List<ConfigGenerator>();
            Apns.Add(new ConfigGenerator
            {
                Name = "IIJmio",
                AuthenticationType = "CHAP",
                Apn = "iijmio.jp",
                Username = "mio@iij",
                Password = "iij",
            });
            Apns.Add(new ConfigGenerator
            {
                Name = "DMM Mobile",
                AuthenticationType = "CHAP",
                Apn = "dmm.com",
                Username = "a",
                Password = "a",
            });
            Apns.Add(new ConfigGenerator
            {
                Name = "DMM Mobile (vmobile)",
                AuthenticationType = "CHAP",
                Apn = "vmobile.jp",
                Username = "a",
                Password = "a",
            });
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Apns);
            using (var writer = new StreamWriter("./apns.json"))
            {
                writer.Write(json);
            }
        }

        public void Load()
        {
            try
            {
                using (var reader = new StreamReader("./apns.json"))
                {
                    var apns = JsonConvert.DeserializeObject<ConfigGenerator[]>(reader.ReadToEnd());
                    Apns.Clear();
                    Apns.AddRange(apns);
                }
            }
            catch (Exception e)
            {
                // ignore
            }
        }
    }
}