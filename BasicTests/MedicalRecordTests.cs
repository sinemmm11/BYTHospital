using System;
using NUnit.Framework;

namespace HospitalSystem.Tests
{
    public class MedicalRecordTests
    {
        [Test]
        public void Patient_Should_Have_MedicalRecord_After_MakePatient()
        {
            var dept = TestHelper.CreateDepartment();
            var doctor = TestHelper.CreateDoctor(dept);

            var patient = TestHelper.CreatePatient(doctor);

            Assert.That(patient.IsPatient, Is.True);
            Assert.That(patient.MedicalRecord, Is.Not.Null);
            Assert.That(patient.ResponsibleDoctor, Is.EqualTo(doctor));
            Assert.That(doctor.ResponsibleForPatients, Does.Contain(patient));
        }

        [Test]
        public void CompleteAppointment_Should_Create_Consultation_And_Optional_Diagnosis_Prescription()
        {
            var dept = TestHelper.CreateDepartment();
            var doctor = TestHelper.CreateDoctor(dept);
            var patient = TestHelper.CreatePatient(doctor);

            var appt = new Appointment(patient, doctor, DateTime.Now.AddHours(2));
            appt.CompleteAppointment(
                consultationNotes: "Notes",
                diagnosisDesc: "Flu",
                medication: "Paracetamol",
                dosage: "2x"
            );

            Assert.That(appt.Status, Is.EqualTo("Completed"));
            Assert.That(appt.ResultingConsultation, Is.Not.Null);
            Assert.That(appt.Diagnosis, Is.Not.Null);
            Assert.That(appt.Prescription, Is.Not.Null);

            Assert.That(patient.MedicalRecord!.Consultations.Count, Is.GreaterThan(0));
            Assert.That(patient.MedicalRecord!.Diagnoses.Count, Is.GreaterThan(0));
            Assert.That(patient.MedicalRecord!.Prescriptions.Count, Is.GreaterThan(0));
        }
    }
}