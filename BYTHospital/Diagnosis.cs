using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Diagnosis
    {
        public static List<Diagnosis> Extent = new();

        public Patient Patient { get; }
        public Doctor Doctor { get; }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Diagnosis date cannot be in the future.");
                _date = value;
            }
        }

       
        public List<string> IcdCodes { get; set; } = new();

        public Diagnosis(Patient patient, Doctor doctor, string description, DateTime date)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Description = description;
            Date = date;
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Diagnosis>>(File.ReadAllText(file)) ?? new();
        }
    }
}
