using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Appointment
    {
        public static List<Appointment> Extent = new();

        private Patient _patient = default!;
        public Patient Patient
        {
            get => _patient;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Patient));
                if (_patient == value) return;

                _patient?.RemoveAppointment(this);
                _patient = value;
                _patient.AddAppointment(this);
            }
        }

        private Doctor _doctor = default!;
        public Doctor Doctor
        {
            get => _doctor;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Doctor));
                if (_doctor == value) return;

                _doctor?.RemoveConductedAppointment(this);
                _doctor = value;
                _doctor.AddConductedAppointment(this);
            }
        }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if (value < System.DateTime.Now)
                    throw new ArgumentException("Appointment cannot be in the past.");
                _dateTime = value;
            }
        }

        private string _status = "Scheduled";
        public string Status
        {
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be empty.");
                _status = value;
            }
        }

        private Diagnosis? _diagnosis;
        public Diagnosis? Diagnosis
        {
            get => _diagnosis;
            set
            {
                if (_diagnosis == value) return;
                var oldDiag = _diagnosis;
                _diagnosis = value;
                if (oldDiag != null) oldDiag.SourceAppointment = null;
                if (_diagnosis != null) _diagnosis.SourceAppointment = this;
            }
        }

        private Prescription? _prescription;
        public Prescription? Prescription
        {
            get => _prescription;
            set
            {
                if (_prescription == value) return;
                var oldPresc = _prescription;
                _prescription = value;
                if (oldPresc != null) oldPresc.SourceAppointment = null;
                if (_prescription != null) _prescription.SourceAppointment = this;
            }
        }

        private Consultation? _resultingConsultation;
        public Consultation? ResultingConsultation
        {
            get => _resultingConsultation;
            set
            {
                if (_resultingConsultation == value) return;
                var oldConsult = _resultingConsultation;
                _resultingConsultation = value;
                if (oldConsult != null) oldConsult.SourceAppointment = null;
                if (_resultingConsultation != null) _resultingConsultation.SourceAppointment = this;
            }
        }

        public List<Nurse> AssistingNurses { get; } = new();

        public void AddAssistingNurse(Nurse nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            if (!AssistingNurses.Contains(nurse))
            {
                AssistingNurses.Add(nurse);
                nurse.AddAssistedAppointment(this);
            }
        }

        public void RemoveAssistingNurse(Nurse nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            if (AssistingNurses.Contains(nurse))
            {
                AssistingNurses.Remove(nurse);
                nurse.RemoveAssistedAppointment(this);
            }
        }

        public void CompleteAppointment(string consultationNotes, string? diagnosisDesc = null, string? medication = null, string? dosage = null)
        {
            Status = "Completed";
            
            // Create consultation (if appointment is conducted)
            var consult = new Consultation(this.Patient.MedicalRecord, DateTime.Now, consultationNotes);
            ResultingConsultation = consult;

            // Optional diagnosis
            if (!string.IsNullOrEmpty(diagnosisDesc))
            {
                var diag = new Diagnosis(this.Patient.MedicalRecord, diagnosisDesc, DateTime.Now);
                diag.Consultation = consult; // Link to Consultation
                Diagnosis = diag;
            }

            // Optional prescription
            if (!string.IsNullOrEmpty(medication) && !string.IsNullOrEmpty(dosage))
            {
                var presc = new Prescription(this.Patient.MedicalRecord);
                presc.Medication = medication;
                presc.Dosage = dosage;
                presc.Consultation = consult; // Link to Consultation
                Prescription = presc;
            }
        }

       
        public Appointment(Patient patient, Doctor doctor, DateTime dateTime)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));

            _dateTime = dateTime;
            _patient = patient;
            _doctor = doctor;
            
            _patient.AddAppointment(this);
            _doctor.AddConductedAppointment(this);

            Status = "Scheduled";

            Extent.Add(this);
        }

       
        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<Appointment>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(file));
            Extent = data ?? new List<Appointment>();
        }
    }
}
