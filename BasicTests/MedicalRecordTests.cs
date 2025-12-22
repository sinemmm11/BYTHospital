using NUnit.Framework;
using HospitalSystem;
using System;
using System.Linq;

namespace HospitalSystem.Tests
{
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

        [Test]
        public void PatientCreation_CreatesMedicalRecord()
        {
            Assert.That(_patient.MedicalRecord, Is.Not.Null);
            Assert.That(_patient.MedicalRecord.Patient, Is.EqualTo(_patient));
        }

        [Test]
        public void GetAllRecordsChronologically_ReturnsCorrectOrder()
        {
            var now = DateTime.Now;

            var diagnosisEvent = new Diagnosis(_patient.MedicalRecord, "Flu", now.AddDays(-1)); 
            var consultationEvent = new Consultation(_patient.MedicalRecord, now.AddHours(-1), "Checkup"); 
            var appointmentEvent = new Appointment(_patient, _doctor, now.AddHours(1)); 
            var surgeryEvent = new Surgery(_patient, _doctor, now.AddHours(2)); 
            
            _patient.MedicalRecord.Surgeries.Add(surgeryEvent);

            var records = _patient.MedicalRecord.GetAllRecordsChronologically().ToList();

            Assert.That(records[0], Is.EqualTo(diagnosisEvent));
            Assert.That(records[1], Is.EqualTo(consultationEvent));
            Assert.That(records[2], Is.EqualTo(appointmentEvent));
            Assert.That(records[3], Is.EqualTo(surgeryEvent));
        }
    }
}
