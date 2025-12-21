using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalSystem
{
    public class MedicalRecord
    {
        public Patient Patient { get; }

        public List<Appointment> Appointments => Patient.Appointments;
        public List<Surgery> Surgeries => Patient.Surgeries;

        public List<Consultation> Consultations { get; } = new();
        public List<Diagnosis> Diagnoses { get; } = new();
        public List<Prescription> Prescriptions { get; } = new();

        public MedicalRecord(Patient patient)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
        }

        public IEnumerable<object> GetAllRecordsChronologically()
        {
            var allRecords = new List<object>();
            allRecords.AddRange(Appointments);
            allRecords.AddRange(Surgeries);
            allRecords.AddRange(Consultations);
            allRecords.AddRange(Diagnoses);
            allRecords.AddRange(Prescriptions);

            return allRecords.OrderBy(r =>
            {
                return r switch
                {
                    Appointment a => a.DateTime,
                    Consultation c => c.Date,
                    Surgery s => s.StartTime,
                    Diagnosis d => d.Date,
                    Prescription p => p.DateIssued,
                    _ => DateTime.MinValue
                };
            });
        }
    }
}
