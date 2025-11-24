using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Consultation
    {
        public static List<Consultation> Extent = new List<Consultation>();

        public Patient Patient { get; }
        public Doctor Doctor { get; }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now.AddDays(1))
                    throw new ArgumentException("Consultation date cannot be too far in the future.");
                _date = value;
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Notes cannot be empty.");
                _notes = value;
            }
        }

        // Senin attribute’un (istersen tut)
        private string _recommendations = string.Empty;
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

        public Consultation(Patient patient, Doctor doctor, DateTime date, string notes)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Date = date;
            Notes = notes;
            Recommendations = "General";

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
                Extent = new List<Consultation>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Consultation>>(File.ReadAllText(file));
            Extent = data ?? new List<Consultation>();
        }
    }
}