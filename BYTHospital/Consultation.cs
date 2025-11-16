using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Consultation
    {
        public static List<Consultation> Extent = new List<Consultation>();

        private string _recommendations;
        public string Recommendations
        {
            get => _recommendations;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Recommendations cannot be empty.");
                _recommendations = value;
            }
        }

        public Consultation()
        {
            Extent.Add(this);
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (File.Exists(file))
                Extent = JsonSerializer.Deserialize<List<Consultation>>(File.ReadAllText(file));
        }
    }
}