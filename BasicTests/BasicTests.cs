using NUnit.Framework;
using HospitalSystem;
using System;

namespace HospitalSystem.Tests
{
    public class BasicTests
    {
        [Test]
        public void Patient_NameCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.Name = "");
        }

        [Test]
        public void Patient_AgeCannotBeNegative()
        {
            var p = new Patient();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Age = -1);
        }

        [Test]
        public void Patient_ExtentIncreases()
        {
            int before = Patient.Extent.Count;
            var p = new Patient();
            Assert.That(Patient.Extent.Count, Is.EqualTo(before + 1));
        }

        [Test]
        public void Doctor_SpecializationCannotBeEmpty()
        {
            var d = new Doctor();
            Assert.Throws<ArgumentException>(() => d.Specialization = "");
        }

        [Test]
        public void Doctor_LicenseNumberCannotBeEmpty()
        {
            var d = new Doctor();
            Assert.Throws<ArgumentException>(() => d.LicenseNumber = "");
        }

        [Test]
        public void Doctor_ExtentIncreases()
        {
            int before = Doctor.Extent.Count;
            var d = new Doctor();
            Assert.That(Doctor.Extent.Count, Is.EqualTo(before + 1));
        }

        [Test]
        public void Appointment_CannotBeInPast()
        {
            var a = new Appointment();
            Assert.Throws<ArgumentException>(() =>
                a.DateTime = DateTime.Now.AddMinutes(-1));
        }

        [Test]
        public void Appointment_StatusCannotBeEmpty()
        {
            var a = new Appointment();
            Assert.Throws<ArgumentException>(() => a.Status = "");
        }

        [Test]
        public void Room_CapacityMustBeGreaterThanZero()
        {
            var r = new Room();
            Assert.Throws<ArgumentOutOfRangeException>(() => r.Capacity = 0);
        }

        [Test]
        public void Prescription_MedicationCannotBeEmpty()
        {
            var pr = new Prescription();
            Assert.Throws<ArgumentException>(() => pr.Medication = "");
        }

        [Test]
        public void Prescription_DosageCannotBeEmpty()
        {
            var pr = new Prescription();
            Assert.Throws<ArgumentException>(() => pr.Dosage = "");
        }
    }
}