using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Consultation
    {
        public static List<Consultation> Extent = new List<Consultation>();

        private MedicalRecord _medicalRecord = default!;
        public MedicalRecord MedicalRecord
        {
            get => _medicalRecord;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(MedicalRecord));
                if (_medicalRecord == value) return;

                _medicalRecord?.Consultations.Remove(this);
                _medicalRecord = value;
                _medicalRecord.Consultations.Add(this);
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now.AddDays(1))
                    throw new ArgumentException("Consultation date cannot be too far in the future.");
                _date = value;
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Notes cannot be empty.");
                _notes = value;
            }
        }

        
        private string _recommendations = string.Empty;
        public string Recommendations
        {
            get => _recommendations;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Recommendations cannot be empty.");
                _recommendations = value;
            }
        }

        private Appointment? _sourceAppointment;
        public Appointment? SourceAppointment
        {
            get => _sourceAppointment;
            set
            {
                if (_sourceAppointment == value) return;
                var oldAppt = _sourceAppointment;
                _sourceAppointment = value;
                if (oldAppt != null) oldAppt.ResultingConsultation = null;
                if (_sourceAppointment != null) _sourceAppointment.ResultingConsultation = this;
            }
        }

        public List<Diagnosis> Diagnoses { get; } = new();
        public List<Prescription> Prescriptions { get; } = new();

        internal void InternalAddDiagnosis(Diagnosis d)
        {
            if (!Diagnoses.Contains(d)) Diagnoses.Add(d);
        }

        internal void InternalRemoveDiagnosis(Diagnosis d)
        {
            Diagnoses.Remove(d);
        }

        internal void InternalAddPrescription(Prescription p)
        {
            if (!Prescriptions.Contains(p)) Prescriptions.Add(p);
        }

        internal void InternalRemovePrescription(Prescription p)
        {
            Prescriptions.Remove(p);
        }

        public Consultation(MedicalRecord record, DateTime date, string notes)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            _medicalRecord = record;
            
            _medicalRecord.Consultations.Add(this);

            Date = date;
            Notes = notes;
            Recommendations = "General";

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
                Extent = new List<Consultation>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Consultation>>(File.ReadAllText(file));
            Extent = data ?? new List<Consultation>();
        }
    }
}
