using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HospitalSystem
{
    public class Appointment
    {
        public static List<Appointment> Extent = new();

        private Person _patient = default!;
        public Person Patient
        {
            get => _patient;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Patient));
                if (!value.IsPatient) throw new ArgumentException("Appointment patient must be IsPatient=true.");
                if (_patient == value) return;

                _patient?.InternalRemoveAppointment(this);
                _patient = value;
                _patient.InternalAddAppointment(this);
            }
        }

        private Person _doctor = default!;
        public Person Doctor
        {
            get => _doctor;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Doctor));
                if (!value.IsDoctor) throw new ArgumentException("Appointment doctor must be a Doctor (IsDoctor=true).");
                if (_doctor == value) return;

                _doctor?.InternalRemoveConductedAppointment(this);
                _doctor = value;
                _doctor.InternalAddConductedAppointment(this);
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
                var old = _diagnosis;
                _diagnosis = value;
                if (old != null) old.SourceAppointment = null;
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
                var old = _prescription;
                _prescription = value;
                if (old != null) old.SourceAppointment = null;
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
                var old = _resultingConsultation;
                _resultingConsultation = value;
                if (old != null) old.SourceAppointment = null;
                if (_resultingConsultation != null) _resultingConsultation.SourceAppointment = this;
            }
        }

        // Assisting nurses => Person but must be Nurse
        [JsonIgnore]
        public List<Person> AssistingNurses { get; } = new();

        public void AddAssistingNurse(Person nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            if (!nurse.IsNurse) throw new ArgumentException("Assisting staff must be a Nurse.");

            if (!AssistingNurses.Contains(nurse))
                AssistingNurses.Add(nurse);
        }

        public void RemoveAssistingNurse(Person nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            AssistingNurses.Remove(nurse);
        }

        public void CompleteAppointment(string consultationNotes, string? diagnosisDesc = null, string? medication = null, string? dosage = null)
        {
            Status = "Completed";

            if (Patient.MedicalRecord == null)
                throw new InvalidOperationException("Patient must have a MedicalRecord.");

            var consult = new Consultation(Patient.MedicalRecord, DateTime.Now, consultationNotes);
            ResultingConsultation = consult;

            if (!string.IsNullOrEmpty(diagnosisDesc))
            {
                var diag = new Diagnosis(Patient.MedicalRecord, diagnosisDesc, DateTime.Now);
                diag.Consultation = consult;
                Diagnosis = diag;
            }

            if (!string.IsNullOrEmpty(medication) && !string.IsNullOrEmpty(dosage))
            {
                var presc = new Prescription(Patient.MedicalRecord)
                {
                    Medication = medication,
                    Dosage = dosage,
                    Consultation = consult
                };
                Prescription = presc;
            }
        }

        public Appointment(Person patient, Person doctor, DateTime dateTime)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (!patient.IsPatient) throw new ArgumentException("patient must be IsPatient=true.");
            if (!doctor.IsDoctor) throw new ArgumentException("doctor must be a Doctor.");
            if (dateTime < DateTime.Now) throw new ArgumentException("Appointment cannot be in the past.");

            _dateTime = dateTime;
            _patient = patient;
            _doctor = doctor;

            _patient.InternalAddAppointment(this);
            _doctor.InternalAddConductedAppointment(this);

            Status = "Scheduled";
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Appointment>>(File.ReadAllText(file)) ?? new();
        }
    }
}