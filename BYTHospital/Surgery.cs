using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Surgery
    {
        public static List<Surgery> Extent = new();

        public Patient Patient { get; }
        public SurgeonDoctor Surgeon { get; }

        
        public List<SurgeryStaffParticipation> Staff { get; } = new();

        private string _type = "General";
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

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                if (value > DateTime.Now.AddYears(1))
                    throw new ArgumentException("Start time is unrealistic.");
                _startTime = value;
            }
        }

        public DateTime? EndTime { get; private set; }

        public TimeSpan? Duration =>
            EndTime.HasValue ? EndTime.Value - StartTime : (TimeSpan?)null;

        public Surgery(Patient patient, SurgeonDoctor surgeon, DateTime startTime)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Surgeon = surgeon ?? throw new ArgumentNullException(nameof(surgeon));
            StartTime = startTime;
            Type = "General";
            Extent.Add(this);
        }

        public void Finish(DateTime endTime)
        {
            if (endTime < StartTime)
                throw new ArgumentException("End time cannot be before start time");

            EndTime = endTime;
        }

        internal void AddStaffParticipation(SurgeryStaffParticipation p)
        {
            if (!Staff.Contains(p)) Staff.Add(p);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Surgery>>(File.ReadAllText(file)) ?? new();
        }
    }
}
