using NUnit.Framework;
using HospitalSystem;
using System;
using System.IO;

namespace HospitalSystem.Tests
{
    public class BasicTests
    {
        // ======================================
        // 1. ATTRIBUTE VALIDATION TESTLERİ
        // ======================================

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
        public void Patient_BirthDateCannotBeInFuture()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                p.BirthDate = DateTime.Today.AddDays(1);
            });
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

        [Test]
        public void Address_CityCannotBeEmpty()
        {
            var address = new Address();
            Assert.Throws<ArgumentException>(() => address.City = "");
        }

        [Test]
        public void Address_StreetCannotBeEmpty()
        {
            var address = new Address();
            Assert.Throws<ArgumentException>(() => address.Street = "");
        }

        [Test]
        public void Nurse_RegistrationNumberCannotBeEmpty()
        {
            var n = new Nurse();
            Assert.Throws<ArgumentException>(() => n.RegistrationNumber = "");
        }

        [Test]
        public void Diagnosis_DescriptionCannotBeEmpty()
        {
            var p = new Patient();
            var d = new Doctor();

            Assert.Throws<ArgumentException>(() =>
            {
                var diag = new Diagnosis(p, d, "", DateTime.Today);
            });
        }

        [Test]
        public void Diagnosis_DateCannotBeInFuture()
        {
            var p = new Patient();
            var d = new Doctor();

            Assert.Throws<ArgumentException>(() =>
            {
                var diag = new Diagnosis(p, d, "Flu", DateTime.Today.AddDays(1));
            });
        }

        [Test]
        public void Consultation_NotesCannotBeEmpty()
        {
            var p = new Patient();
            var d = new Doctor();

            Assert.Throws<ArgumentException>(() =>
            {
                var c = new Consultation(p, d, DateTime.Now, "");
            });
        }

        [Test]
        public void Consultation_DateTooFarInFutureThrows()
        {
            var p = new Patient();
            var d = new Doctor();

            Assert.Throws<ArgumentException>(() =>
            {
                var c = new Consultation(p, d, DateTime.Now.AddDays(2), "Check-up");
            });
        }

        [Test]
        public void RoomAssignment_DischargeCannotBeBeforeAdmission()
        {
            var room = new Room();
            var patient = new Patient();
            var admission = DateTime.Today;

            var assignment = new RoomAssignment(patient, room, admission);

            Assert.Throws<ArgumentException>(() =>
            {
                assignment.Discharge(admission.AddDays(-1));
            });
        }

        [Test]
        public void Surgery_FinishCannotBeBeforeStartTime()
        {
            var p = new Patient();
            var sDoc = new SurgeonDoctor();
            var start = DateTime.Now;

            var surgery = new Surgery(p, sDoc, start);

            Assert.Throws<ArgumentException>(() =>
            {
                surgery.Finish(start.AddMinutes(-10));
            });
        }


        // ======================================
        // 2. MULTI-VALUE ATTRIBUTE TESTLERİ
        // ======================================

        [Test]
        public void Patient_AddAllergy_AddsToList()
        {
            var p = new Patient();
            int before = p.Allergies.Count;

            p.AddAllergy("Penicillin");

            Assert.That(p.Allergies, Contains.Item("Penicillin"));
            Assert.That(p.Allergies.Count, Is.EqualTo(before + 1));
        }

        [Test]
        public void Patient_AddAllergy_CannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.AddAllergy(""));
        }

        [Test]
        public void Patient_AddAllergy_DoesNotAddDuplicate()
        {
            var p = new Patient();
            p.AddAllergy("Penicillin");
            int before = p.Allergies.Count;

            p.AddAllergy("Penicillin");

            Assert.That(p.Allergies.Count, Is.EqualTo(before));
        }

        [Test]
        public void Department_AddDoctor_SetsBidirectionalRelation()
        {
            var dep = new Department("Cardiology");
            var doc = new Doctor();

            dep.AddDoctor(doc);

            Assert.That(dep.Doctors, Contains.Item(doc));
            Assert.That(doc.Department, Is.EqualTo(dep));
        }

        [Test]
        public void Department_AddNurse_SetsBidirectionalRelation()
        {
            var dep = new Department("Cardiology");
            var nurse = new Nurse();

            dep.AddNurse(nurse);

            Assert.That(dep.Nurses, Contains.Item(nurse));
            Assert.That(nurse.Department, Is.EqualTo(dep));
        }

        [Test]
        public void Room_IsFull_WhenAssignmentsReachCapacity()
        {
            var room = new Room(); // Capacity default: 1
            var patient = new Patient();

            var assignment = new RoomAssignment(patient, room, DateTime.Today);

            Assert.That(room.IsFull, Is.True);
        }

        [Test]
        public void Room_AddAssignmentThrowsWhenFull()
        {
            var room = new Room(); // Capacity = 1
            var p1 = new Patient();
            var p2 = new Patient();

            var a1 = new RoomAssignment(p1, room, DateTime.Today);

            Assert.That(room.IsFull, Is.True);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var a2 = new RoomAssignment(p2, room, DateTime.Today);
            });
        }


        // ======================================
        // 3. OPTIONAL ATTRIBUTE TESTLERİ
        // ======================================

        [Test]
        public void Patient_MiddleNameCanBeNull()
        {
            var p = new Patient();
            p.SetMiddleName(null);

            Assert.That(p.MiddleName, Is.Null);
        }

        [Test]
        public void Patient_MiddleNameCannotBeOnlySpaces()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.SetMiddleName("   "));
        }

        [Test]
        public void Prescription_Instructions_CanBeNull()
        {
            var pr = new Prescription();
            pr.Instructions = null;

            Assert.That(pr.Instructions, Is.Null);
        }


        // ======================================
        // 4. DERIVED ATTRIBUTE TESTLERİ
        // ======================================

        [Test]
        public void Patient_CalculatedAge_IsCorrect()
        {
            var p = new Patient();
            p.BirthDate = new DateTime(2000, 1, 1);

            int expected = DateTime.Today.Year - 2000;
            if (new DateTime(DateTime.Today.Year, 1, 1) > DateTime.Today)
                expected--;

            Assert.That(p.CalculatedAge, Is.EqualTo(expected));
        }

        [Test]
        public void RoomAssignment_StayLengthInDays_IsCorrect()
        {
            var room = new Room();
            var patient = new Patient();

            var admissionDate = new DateTime(2024, 1, 1);
            var dischargeDate = new DateTime(2024, 1, 11); // 10 days

            var assignment = new RoomAssignment(patient, room, admissionDate);
            assignment.Discharge(dischargeDate);

            Assert.That(assignment.StayLengthInDays, Is.EqualTo(10));
        }

        [Test]
        public void Surgery_FinishSetsEndTimeAndPositiveDuration()
        {
            var p = new Patient();
            var sDoc = new SurgeonDoctor();
            var start = DateTime.Now;

            var surgery = new Surgery(p, sDoc, start);
            var end = start.AddHours(2);

            surgery.Finish(end);

            Assert.That(surgery.EndTime, Is.EqualTo(end));
            Assert.That(surgery.Duration.HasValue, Is.True);
            Assert.That(surgery.Duration!.Value.TotalMinutes, Is.GreaterThan(0));
        }


        // ======================================
        // 5. STATIC / CLASS ATTRIBUTE (EXTENT)
        // ======================================

        [Test]
        public void Patient_ExtentIncreases()
        {
            int before = Patient.Extent.Count;
            var p = new Patient();
            Assert.That(Patient.Extent.Count, Is.EqualTo(before + 1));
        }

        [Test]
        public void Doctor_ExtentIncreases()
        {
            int before = Doctor.Extent.Count;
            var d = new Doctor();
            Assert.That(Doctor.Extent.Count, Is.EqualTo(before + 1));
        }

        [Test]
        public void Appointment_ExtentIncreases()
        {
            int before = Appointment.Extent.Count;
            var a = new Appointment();
            Assert.That(Appointment.Extent.Count, Is.EqualTo(before + 1));
        }


        // ======================================
        // 6. EXTENT PERSISTENCE (SAVE + LOAD)
        // ======================================

        [Test]
        public void Patient_SaveAndLoadExtent_PreservesSavedCount()
        {
            string path = "patients_test.json";

            // Save öncesi: mevcut extent + iki yeni hasta
            var p1 = new Patient();
            var p2 = new Patient();
            int savedCount = Patient.Extent.Count;

            Patient.SaveExtent(path);

            // Save sonrası fazladan bir hasta daha eklensin (extent değişsin)
            var p3 = new Patient();
            Assert.That(Patient.Extent.Count, Is.GreaterThan(savedCount));

            // Load çağırınca save anındaki sayıya dönmeli
            Patient.LoadExtent(path);

            Assert.That(Patient.Extent.Count, Is.EqualTo(savedCount));

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}