using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Surgery
    {
        public static List<Surgery> Extent = new List<Surgery>();

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surgery type cannot be empty.");
                _type = value;
            }
        }

        public DateTime StartTime { get; set; }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Duration), "Duration must be > 0.");
                _duration = value;
            }
        }

        public DateTime EndTime { get; set; }

        public Surgery()
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
                Extent = JsonSerializer.Deserialize<List<Surgery>>(File.ReadAllText(file));
        }
    }
}