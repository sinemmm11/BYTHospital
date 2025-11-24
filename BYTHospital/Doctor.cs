using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Doctor : Employee
    {
        public static List<Doctor> Extent = new List<Doctor>();

        private string _specialization = "General";
        public string Specialization
        {
            get => _specialization;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Specialization cannot be empty.");
                _specialization = value;
            }
        }

        private string _licenseNumber = "00000";
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("License number cannot be empty.");
                _licenseNumber = value;
            }
        }

        // Department ile bidirectional ilişki için
        public Department? Department { get; set; }

        public Doctor()
        {
            Specialization = "General";
            LicenseNumber = "00000";
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
                Extent = new List<Doctor>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Doctor>>(File.ReadAllText(file));
            Extent = data ?? new List<Doctor>();
        }
    }
}