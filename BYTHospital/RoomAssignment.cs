using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class RoomAssignment
    {
        public static List<RoomAssignment> Extent = new List<RoomAssignment>();

        public Patient Patient { get; }
        public Room Room { get; }

        private DateTime _admissionDate;
        public DateTime AdmissionDate
        {
            get => _admissionDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Admission date cannot be in the future");
                _admissionDate = value;
            }
        }

        // optional
        public DateTime? DischargeDate { get; private set; }

        // Derived
        public int StayLengthInDays
        {
            get
            {
                var end = DischargeDate ?? DateTime.Today;
                return (end.Date - AdmissionDate.Date).Days;
            }
        }

        public RoomAssignment(Patient patient, Room room, DateTime admissionDate)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            AdmissionDate = admissionDate;

            Extent.Add(this);
            room.AddAssignment(this);
        }

        public void Discharge(DateTime dischargeDate)
        {
            if (dischargeDate < AdmissionDate)
                throw new ArgumentException("Discharge date cannot be before admission date");
            DischargeDate = dischargeDate;
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<RoomAssignment>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<RoomAssignment>>(File.ReadAllText(file));
            Extent = data ?? new List<RoomAssignment>();
        }
    }
}