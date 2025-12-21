using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HospitalSystem
{
    public class Patient : Person
    {
        public static List<Patient> Extent { get; private set; } = new();
        private static int _totalRegisteredPatients = 0;
        public static int TotalRegisteredPatients => _totalRegisteredPatients;

        private Doctor? _responsibleDoctor;
        public Doctor ResponsibleDoctor
        {
            get => _responsibleDoctor ?? throw new InvalidOperationException("Patient must have a responsible doctor.");
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value), "Responsible doctor cannot be null.");
                if (_responsibleDoctor == value) return;

                _responsibleDoctor?.InternalRemoveResponsiblePatient(this);
                _responsibleDoctor = value;
                _responsibleDoctor.InternalAddResponsiblePatient(this);
            }
        }

        public void RemoveResponsibleDoctor()
        {
            if (_responsibleDoctor != null)
            {
                var oldDoc = _responsibleDoctor;
                _responsibleDoctor = null;
                oldDoc.InternalRemoveResponsiblePatient(this);
            }
        }

       
        public List<Appointment> Appointments { get; } = new();

        internal void AddAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (!Appointments.Contains(a))
                Appointments.Add(a);
        }

        internal void RemoveAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            Appointments.Remove(a);
        }

        public List<Surgery> Surgeries { get; } = new();
        public List<RoomAssignment> RoomAssignments { get; } = new();

        internal void AddSurgery(Surgery s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!Surgeries.Contains(s)) Surgeries.Add(s);
        }
        
        internal void AddRoomAssignment(RoomAssignment ra)
        {
            if (ra == null) throw new ArgumentNullException(nameof(ra));
            if (!RoomAssignments.Contains(ra)) RoomAssignments.Add(ra);
        }

        public bool HasActiveSurgery() => Surgeries.Any(s => s.EndTime == null);
        public bool HasActiveRoomAssignment() => RoomAssignments.Any(ra => ra.DischargeDate == null);
        
        private DateTime _birthDate = DateTime.Today.AddYears(-18);
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Today)
                    throw new ArgumentException("Birth date cannot be in the future");
                _birthDate = value;
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public string? MiddleName { get; private set; }
        public void SetMiddleName(string? middleName)
        {
            if (middleName != null && string.IsNullOrWhiteSpace(middleName))
                throw new ArgumentException("Middle name cannot be only whitespace");
            MiddleName = middleName;
        }

        public List<string> Allergies { get; } = new();
        public void AddAllergy(string allergy)
        {
            if (string.IsNullOrWhiteSpace(allergy))
                throw new ArgumentException("Allergy cannot be empty");

            if (!Allergies.Contains(allergy))
                Allergies.Add(allergy);
        }

        public MedicalRecord MedicalRecord { get; private set; }

        public Patient()
        {
            Name = "Unknown";
            Surname = "Unknown";
            NationalID = "00000000000";
            Gender = "Unknown";
            PhoneNumber = "000000000";
            MedicalRecord = new MedicalRecord(this); 
            _totalRegisteredPatients++;
            Extent.Add(this);
        }

        
        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, Extent.Count.ToString());
        }

        public static void LoadExtent(string file)
        {
            Extent = new List<Patient>();

            if (!File.Exists(file))
                return;

            var text = File.ReadAllText(file);
            if (int.TryParse(text, out int count) && count >= 0)
            {
                for (int i = 0; i < count; i++)
                    new Patient();
            }
        }
    }
}
