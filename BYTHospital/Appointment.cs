using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Appointment
    {
        public static List<Appointment> Extent = new List<Appointment>();

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if (value < System.DateTime.Now)
                    throw new ArgumentException("Appointment cannot be in the past.");
                _dateTime = value;
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be empty.");
                _status = value;
            }
        }

        public Appointment()
        {
            DateTime = System.DateTime.Now.AddHours(1);
            Status = "Scheduled";
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
                Extent = new List<Appointment>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(file));
            Extent = data ?? new List<Appointment>();
        }
    }
}