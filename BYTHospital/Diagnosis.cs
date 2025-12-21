using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Diagnosis
    {
        public static List<Diagnosis> Extent = new();

        private MedicalRecord _medicalRecord = default!;
        public MedicalRecord MedicalRecord
        {
            get => _medicalRecord;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(MedicalRecord));
                if (_medicalRecord == value) return;

                _medicalRecord?.Diagnoses.Remove(this);
                _medicalRecord = value;
                _medicalRecord.Diagnoses.Add(this);
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
                if (oldAppt != null) oldAppt.Diagnosis = null;
                if (_sourceAppointment != null) _sourceAppointment.Diagnosis = this;
            }
        }

        private Consultation? _consultation;
        public Consultation? Consultation
        {
            get => _consultation;
            set
            {
                if (_consultation == value) return;
                var oldConsult = _consultation;
                _consultation = value;
                if (oldConsult != null) oldConsult.InternalRemoveDiagnosis(this);
                if (_consultation != null) _consultation.InternalAddDiagnosis(this);
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Diagnosis date cannot be in the future.");
                _date = value;
            }
        }

       
        public List<string> IcdCodes { get; set; } = new();

        public Diagnosis(MedicalRecord record, string description, DateTime date)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            _medicalRecord = record;
            _medicalRecord.Diagnoses.Add(this);

            Description = description;
            Date = date;
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Diagnosis>>(File.ReadAllText(file)) ?? new();
        }
    }
}
