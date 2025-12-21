using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Prescription
    {
        public static List<Prescription> Extent = new();

        private string _medication = "Unknown";
        public string Medication
        {
            get => _medication;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Medication cannot be empty.");
                _medication = value;
            }
        }

        private string _dosage = "Unknown";
        public string Dosage
        {
            get => _dosage;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Dosage cannot be empty.");
                _dosage = value;
            }
        }

       
        public string? Instructions { get; set; }
        public DateTime DateIssued { get; private set; }

        private Appointment? _sourceAppointment;
        public Appointment? SourceAppointment
        {
            get => _sourceAppointment;
            set
            {
                if (_sourceAppointment == value) return;
                var oldAppt = _sourceAppointment;
                _sourceAppointment = value;
                if (oldAppt != null) oldAppt.Prescription = null;
                if (_sourceAppointment != null) _sourceAppointment.Prescription = this;
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
                if (oldConsult != null) oldConsult.InternalRemovePrescription(this);
                if (_consultation != null) _consultation.InternalAddPrescription(this);
            }
        }

        private MedicalRecord _medicalRecord = default!;
        public MedicalRecord MedicalRecord
        {
            get => _medicalRecord;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(MedicalRecord));
                if (_medicalRecord == value) return;

                _medicalRecord?.Prescriptions.Remove(this);
                _medicalRecord = value;
                _medicalRecord.Prescriptions.Add(this);
            }
        }

        public Prescription(MedicalRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            _medicalRecord = record;
            
            _medicalRecord.Prescriptions.Add(this);

            DateIssued = DateTime.Now;
            Extent.Add(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Prescription>>(File.ReadAllText(file)) ?? new();
        }
    }
}
