using System;
using System.IO;
using System.Text.Json;

namespace atomic_clock
{
    // Classe manuale per salvare le impostazioni senza usare l'interfaccia di VS
    public class AppSettings
    {
        public double WindowLeft { get; set; } = 100;
        public double WindowTop { get; set; } = 100;
        public double WindowOpacity { get; set; } = 1.0;
        public string LastServer { get; set; } = "time.google.com";
    }

    public static class SettingsManager
    {
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        public static AppSettings Data { get; set; } = new AppSettings();

        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    Data = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                catch { Data = new AppSettings(); }
            }
        }

        public static void Save()
        {
            string json = JsonSerializer.Serialize(Data);
            File.WriteAllText(_filePath, json);
        }
    }
}