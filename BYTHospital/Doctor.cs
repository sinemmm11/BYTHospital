using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Doctor : Employee
    {
        public static List<Doctor> Extent = new();

        private string _specialization = "General";
        public string Specialization
        {
            get => _specialization;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Specialization cannot be empty.");
                _specialization = value;
            }
        }

        private string _licenseNumber = "00000";
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("License number cannot be empty.");
                _licenseNumber = value;
            }
        }

       
        public List<Patient> ResponsibleForPatients { get; } = new();

        public void AddResponsiblePatient(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (!ResponsibleForPatients.Contains(patient))
            {
                ResponsibleForPatients.Add(patient);
                patient.AddResponsibleDoctor(this);
            }
        }

        public void RemoveResponsiblePatient(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (ResponsibleForPatients.Remove(patient))
                patient.RemoveResponsibleDoctor(this);
        }

        
        public List<Appointment> ConductedAppointments { get; } = new();

        internal void AddConductedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (!ConductedAppointments.Contains(a))
                ConductedAppointments.Add(a);
        }

        internal void RemoveConductedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            ConductedAppointments.Remove(a);
        }

        public Doctor()
        {
            Specialization = "General";
            LicenseNumber = "00000";
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Doctor>>(File.ReadAllText(file)) ?? new();
        }
    }
}
