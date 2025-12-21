using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HospitalSystem
{
    public class Doctor : Employee, IConsultant, ISurgeon
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

        private string _consultingHours = "N/A";
        public string ConsultingHours
        {
            get => _consultingHours;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Consulting hours cannot be empty.");
                _consultingHours = value;
            }
        }

        private string _surgeonSpeciality = "General Surgery";
        public string SurgeonSpeciality
        {
            get => _surgeonSpeciality;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surgeon speciality cannot be empty.");
                _surgeonSpeciality = value;
            }
        }

        private Doctor? _supervisingDoctor;
        public Doctor? SupervisingDoctor
        {
            get => _supervisingDoctor;
            set
            {
                if (value == this) throw new ArgumentException("A doctor cannot supervise themselves.");
                if (value == _supervisingDoctor) return;

                if (value != null && _supervisingDoctor != null)
                {
                    throw new InvalidOperationException("This doctor is already supervised by another doctor. Remove the current supervisor first.");
                }

                _supervisingDoctor?.InternalRemoveSupervisedDoctor(this);
                _supervisingDoctor = value;
                _supervisingDoctor?.InternalAddSupervisedDoctor(this);
            }
        }

        public List<Doctor> SupervisedDoctors { get; } = new();

        public void AddSupervisedDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            doctor.SupervisingDoctor = this;
        }

        public void RemoveSupervisedDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (doctor.SupervisingDoctor == this)
            {
                doctor.SupervisingDoctor = null;
            }
        }

        internal void InternalAddSupervisedDoctor(Doctor doctor)
        {
            if (!SupervisedDoctors.Contains(doctor)) SupervisedDoctors.Add(doctor);
        }

        internal void InternalRemoveSupervisedDoctor(Doctor doctor)
        {
            SupervisedDoctors.Remove(doctor);
        }

       
        public List<Patient> ResponsibleForPatients { get; } = new();

        internal void InternalAddResponsiblePatient(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (!ResponsibleForPatients.Contains(patient))
                ResponsibleForPatients.Add(patient);
        }

        internal void InternalRemoveResponsiblePatient(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            ResponsibleForPatients.Remove(patient);
        }

        public Dictionary<DateTime, Appointment> ConductedAppointments { get; } = new();

        internal void AddConductedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            
            if (ConductedAppointments.ContainsKey(a.DateTime))
            {
                throw new InvalidOperationException("This doctor already has an appointment at that time.");
            }
            
            ConductedAppointments[a.DateTime] = a;
        }

        internal void RemoveConductedAppointment(Appointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (ConductedAppointments.TryGetValue(a.DateTime, out var existing) && existing == a)
            {
                ConductedAppointments.Remove(a.DateTime);
            }
        }

        public IEnumerable<Appointment> GetAppointmentsChronologically()
        {
            return ConductedAppointments.Values.OrderBy(a => a.DateTime);
        }

        public Doctor()
        {
            Specialization = "General";
            LicenseNumber = "00000";
            ConsultingHours = "N/A"; // Initialize new property
            SurgeonSpeciality = "General Surgery"; // Initialize new property
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
