using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Surgery
    {
        public static List<Surgery> Extent = new();

        private Person _patient = default!;
        public Person Patient
        {
            get => _patient;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Patient));
                if (!value.IsPatient) throw new ArgumentException("Surgery patient must be IsPatient=true.");
                if (_patient == value) return;

                if (value.HasActiveRoomAssignment())
                    throw new InvalidOperationException("Cannot schedule a surgery for a patient who is currently admitted to a room.");

                _patient?.Surgeries.Remove(this);
                _patient = value;
                _patient.InternalAddSurgery(this);
            }
        }

        private Person _surgeon = default!;
        public Person Surgeon
        {
            get => _surgeon;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Surgeon));
                if (!value.IsSurgeon) throw new ArgumentException("Surgeon must be a Doctor with Surgeon role.");
                _surgeon = value;
            }
        }

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

        public TimeSpan Duration { get; set; }
        public DateTime? EndTime => Duration == TimeSpan.Zero ? (DateTime?)null : StartTime.Add(Duration);

        public Surgery(Person patient, Person surgeon, DateTime startTime)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Surgeon = surgeon ?? throw new ArgumentNullException(nameof(surgeon));
            StartTime = startTime;

            Type = "General";
            Duration = TimeSpan.Zero;

            Extent.Add(this);
        }

        internal void AddStaffParticipation(SurgeryStaffParticipation p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
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