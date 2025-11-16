using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Diagnosis
    {
        public static List<Diagnosis> Extent = new List<Diagnosis>();

        public List<string> IcdCodes { get; set; } = new List<string>();

        public Diagnosis()
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
                Extent = JsonSerializer.Deserialize<List<Diagnosis>>(File.ReadAllText(file));
        }
    }
}