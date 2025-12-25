using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HospitalSystem
{
    public class MedicalRecord
    {
        [JsonIgnore]
        public Person Patient { get; }

        [JsonIgnore]
        public List<Appointment> Appointments => Patient.Appointments;

        [JsonIgnore]
        public List<Surgery> Surgeries => Patient.Surgeries;

        public List<Consultation> Consultations { get; } = new();
        public List<Diagnosis> Diagnoses { get; } = new();
        public List<Prescription> Prescriptions { get; } = new();

        public MedicalRecord(Person patient)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            if (!patient.IsPatient)
                throw new InvalidOperationException("MedicalRecord owner must be a Patient (IsPatient=true).");
        }

        public IEnumerable<object> GetAllRecordsChronologically()
        {
            var all = new List<object>();
            all.AddRange(Appointments);
            all.AddRange(Surgeries);
            all.AddRange(Consultations);
            all.AddRange(Diagnoses);
            all.AddRange(Prescriptions);

            return all.OrderBy(r => r switch
            {
                Appointment a => a.DateTime,
                Consultation c => c.Date,
                Surgery s => s.StartTime,
                Diagnosis d => d.Date,
                Prescription p => p.DateIssued,
                _ => DateTime.MinValue
            });
        }
    }
}