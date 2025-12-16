using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Nurse : Employee
    {
        public static List<Nurse> Extent = new();

        private string _registrationNumber = "00000";
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Registration number cannot be empty.");
                _registrationNumber = value;
            }
        }

        private string _shiftDetails = "N/A";
        public string ShiftDetails
        {
            get => _shiftDetails;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Shift details cannot be empty.");
                _shiftDetails = value;
            }
        }

        
        public List<Appointment> AssistedAppointments { get; } = new();

        internal void AddAssistedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (!AssistedAppointments.Contains(a))
                AssistedAppointments.Add(a);
        }

        internal void RemoveAssistedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            AssistedAppointments.Remove(a);
        }

        public Nurse()
        {
            RegistrationNumber = "00000";
            ShiftDetails = "N/A";
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Nurse>>(File.ReadAllText(file)) ?? new();
        }
    }
}
