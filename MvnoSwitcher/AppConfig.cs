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
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Apns);
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            using (var writer = new StreamWriter(Path.Combine(dir, "apns.json")))
            {
                writer.Write(json);
            }
        }

        public void Load()
        {
            try
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                using (var reader = new StreamReader(Path.Combine(dir, "apns.json")))
                {
                    var apns = JsonConvert.DeserializeObject<ConfigGenerator[]>(reader.ReadToEnd());
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