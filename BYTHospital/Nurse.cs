using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Nurse : Person
    {
        public static List<Nurse> Extent = new List<Nurse>();

        // Test için gerekli attribute
        private string _registrationNumber;
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Registration number cannot be empty.");
                _registrationNumber = value;
            }
        }

        // Senin ek attribute’un
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

        public Department? Department { get; set; }

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
            if (!File.Exists(file))
            {
                Extent = new List<Nurse>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Nurse>>(File.ReadAllText(file));
            Extent = data ?? new List<Nurse>();
        }
    }
}