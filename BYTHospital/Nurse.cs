using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Nurse : Person
    {
        public static List<Nurse> Extent = new List<Nurse>();

        private string _shiftDetails;
        public string ShiftDetails
        {
            get => _shiftDetails;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Shift details cannot be empty.");
                _shiftDetails = value;
            }
        }

        public Nurse()
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
                Extent = JsonSerializer.Deserialize<List<Nurse>>(File.ReadAllText(file));
        }
    }
}