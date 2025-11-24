using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Prescription
    {
        public static List<Prescription> Extent = new List<Prescription>();

        private string _medication = "Unknown";
        public string Medication
        {
            get => _medication;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Medication cannot be empty.");
                _medication = value;
            }
        }

        private string _dosage = "Unknown";
        public string Dosage
        {
            get => _dosage;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Dosage cannot be empty.");
                _dosage = value;
            }
        }

        // Optional attribute (test bunu kontrol ediyor)
        public string? Instructions { get; set; }

        public Prescription()
        {
            Extent.Add(this);
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<Prescription>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Prescription>>(File.ReadAllText(file));
            Extent = data ?? new List<Prescription>();
        }
    }
}