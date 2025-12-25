using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HospitalSystem
{
    public class RoomAssignment
    {
        public static List<RoomAssignment> Extent = new();

        private Person _patient = default!;
        public Person Patient
        {
            get => _patient;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Patient));
                if (!value.IsPatient)
                    throw new ArgumentException("RoomAssignment patient must be IsPatient=true.");

                if (_patient == value) return;

                
                if (value.HasActiveSurgery())
                    throw new InvalidOperationException("Cannot admit a patient who is currently in surgery.");

                if (value.HasActiveRoomAssignment())
                    throw new InvalidOperationException("Patient is already admitted to a room.");

               
                _patient?.RoomAssignments.Remove(this);

               
                _patient = value;
                _patient.InternalAddRoomAssignment(this);
            }
        }

        private Room _room = default!;
        public Room Room
        {
            get => _room;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Room));
                if (_room == value) return;

               
                _room?.Assignments.Remove(this);

               
                _room = value;
                _room.AddAssignment(this);
            }
        }

        private DateTime _admissionDate;
        public DateTime AdmissionDate
        {
            get => _admissionDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Admission date cannot be in the future.");
                _admissionDate = value;
            }
        }

        public DateTime? DischargeDate { get; private set; }

        public int StayLengthInDays
        {
            get
            {
                var end = DischargeDate ?? DateTime.Today;
                return (end.Date - AdmissionDate.Date).Days;
            }
        }

     

        public RoomAssignment(Person patient, Room room, DateTime admissionDate)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            AdmissionDate = admissionDate;

            Extent.Add(this);
        }

        public void Discharge(DateTime dischargeDate)
        {
            if (dischargeDate < AdmissionDate)
                throw new ArgumentException("Discharge date cannot be before admission date.");

            DischargeDate = dischargeDate;
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<RoomAssignment>>(File.ReadAllText(file)) ?? new();
        }
    }
}
