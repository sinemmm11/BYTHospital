using System;
using System.Collections.Generic;
using System.IO;

namespace HospitalSystem
{
    public class Patient : Person
    {
        public static List<Patient> Extent { get; private set; } = new();

       
        public List<Doctor> ResponsibleDoctors { get; } = new();

        public void AddResponsibleDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));

            if (!ResponsibleDoctors.Contains(doctor))
            {
                ResponsibleDoctors.Add(doctor);
                doctor.AddResponsiblePatient(this); 
            }
        }

        public void RemoveResponsibleDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));

            if (ResponsibleDoctors.Remove(doctor))
            {
                doctor.RemoveResponsiblePatient(this); 
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

        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Age), "Age cannot be negative.");
                _age = value;
            }
        }

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

        public int CalculatedAge
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

        public Patient()
        {
            
            Name = "Unknown";
            Surname = "Unknown";
            NationalID = "00000000000";
            Gender = "Unknown";
            PhoneNumber = "000000000";

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
