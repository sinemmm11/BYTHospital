using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Appointment
    {
        public static List<Appointment> Extent = new();

        private Patient _patient;
        public Patient Patient
        {
            get => _patient;
            private set => _patient = value ?? throw new ArgumentNullException(nameof(Patient));
        }

        private Doctor _doctor;
        public Doctor Doctor
        {
            get => _doctor;
            private set => _doctor = value ?? throw new ArgumentNullException(nameof(Doctor));
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

       
        public Appointment(Patient patient, Doctor doctor)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

            DateTime = System.DateTime.Now.AddHours(1);
            Status = "Scheduled";

            
            patient.AddAppointment(this);
            doctor.AddConductedAppointment(this);

            Extent.Add(this);
        }

       
        public void ChangeDoctor(Doctor newDoctor)
        {
            if (newDoctor == null) throw new ArgumentNullException(nameof(newDoctor));
            if (newDoctor == Doctor) return;

            Doctor.RemoveConductedAppointment(this);
            Doctor = newDoctor;
            Doctor.AddConductedAppointment(this);
        }

        public void ChangePatient(Patient newPatient)
        {
            if (newPatient == null) throw new ArgumentNullException(nameof(newPatient));
            if (newPatient == Patient) return;

            Patient.RemoveAppointment(this);
            Patient = newPatient;
            Patient.AddAppointment(this);
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
