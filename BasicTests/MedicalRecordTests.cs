using NUnit.Framework;
using HospitalSystem;
using System;
using System.Linq;

namespace HospitalSystem.Tests
{
    // This file specifically tests the MedicalRecord "history hub" logic.
    [TestFixture]
    public class MedicalRecordTests
    {
        private Patient _patient = default!;
        private Doctor _doctor = default!;

        [SetUp]
        public void Setup()
        {
            _patient = new Patient();
            _doctor = new Doctor();
        }

        // Just making sure every patient automatically gets a record when they are created.
        [Test]
        public void PatientCreation_CreatesMedicalRecord()
        {
            Assert.That(_patient.MedicalRecord, Is.Not.Null);
            Assert.That(_patient.MedicalRecord.Patient, Is.EqualTo(_patient));
        }

        // This is the big one: verifying that the record correctly sorts different event types by date.
        [Test]
        public void GetAllRecordsChronologically_ReturnsCorrectOrder()
        {
            var now = DateTime.Now;

            // Creating events at different times to check sorting.
            var diagnosisEvent = new Diagnosis(_patient.MedicalRecord, "Flu", now.AddDays(-1)); // 1 day ago
            var consultationEvent = new Consultation(_patient.MedicalRecord, now.AddHours(-1), "Checkup"); // 1 hour ago
            var appointmentEvent = new Appointment(_patient, _doctor, now.AddHours(1)); // 1 hour from now
            var surgeryEvent = new Surgery(_patient, _doctor, now.AddHours(2)); // 2 hours from now
            
            // Link the surgery to the patient manually since it doesn't use the hub-setter yet.
            _patient.MedicalRecord.Surgeries.Add(surgeryEvent);

            var records = _patient.MedicalRecord.GetAllRecordsChronologically().ToList();

            // Confirming the list comes back in the order we expect (oldest to newest).
            Assert.That(records[0], Is.EqualTo(diagnosisEvent));
            Assert.That(records[1], Is.EqualTo(consultationEvent));
            Assert.That(records[2], Is.EqualTo(appointmentEvent));
            Assert.That(records[3], Is.EqualTo(surgeryEvent));
        }
    }
}