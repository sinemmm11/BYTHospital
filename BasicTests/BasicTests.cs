using NUnit.Framework;
using HospitalSystem;
using System;
using System.IO;

namespace HospitalSystem.Tests
{
    public class BasicTests
    {
      

        [Test]
        public void Person_NameCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.Name = "");
        }

        [Test]
        public void Person_SurnameCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.Surname = "");
        }

        [Test]
        public void Person_NationalIdCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.NationalID = "");
        }

        [Test]
        public void Person_GenderCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.Gender = "");
        }

        [Test]
        public void Person_PhoneNumberCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() => p.PhoneNumber = "");
        }

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
        public void Employee_SalaryCannotBeNegative()
        {
            var d = new Doctor();
            Assert.Throws<ArgumentOutOfRangeException>(() => d.Salary = -100);
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
        public void ConsultantDoctor_ConsultingHoursCannotBeEmpty()
        {
            var cd = new ConsultantDoctor();
            Assert.Throws<ArgumentException>(() => cd.ConsultingHours = "");
        }

        [Test]
        public void SurgeonDoctor_SurgeonSpecialityCannotBeEmpty()
        {
            var sd = new SurgeonDoctor();
            Assert.Throws<ArgumentException>(() => sd.SurgeonSpeciality = "");
        }

        [Test]
        public void ContractorDoctor_SetContractPeriod_ThrowsIfEndDateIsBeforeStartDate()
        {
            var doc = new ContractorDoctor();
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2023, 12, 31);
            Assert.Throws<ArgumentException>(() => doc.SetContractPeriod(startDate, endDate));
        }

        [Test]
        public void PermanentDoctor_SetEmploymentPeriod_ThrowsIfEndDateIsBeforeStartDate()
        {
            var doc = new PermanentDoctor();
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2023, 12, 31);
            Assert.Throws<ArgumentException>(() => doc.SetEmploymentPeriod(startDate, endDate));
        }

        [Test]
        public void Nurse_RegistrationNumberCannotBeEmpty()
        {
            var n = new Nurse();
            Assert.Throws<ArgumentException>(() => n.RegistrationNumber = "");
        }

        [Test]
        public void Nurse_ShiftDetailsCannotBeEmpty()
        {
            var n = new Nurse();
            Assert.Throws<ArgumentException>(() => n.ShiftDetails = "");
        }

        [Test]
        public void Department_NameCannotBeEmpty()
        {
            var d = new Department();
            Assert.Throws<ArgumentException>(() => d.Name = "");
        }

        [Test]
        public void Department_LocationCannotBeEmpty()
        {
            var d = new Department();
            Assert.Throws<ArgumentException>(() => d.Location = "");
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
        public void Room_RoomNumberCannotBeEmpty()
        {
            var r = new Room();
            Assert.Throws<ArgumentException>(() => r.RoomNumber = "");
        }

        [Test]
        public void Room_TypeCannotBeEmpty()
        {
            var r = new Room();
            Assert.Throws<ArgumentException>(() => r.Type = "");
        }

        [Test]
        public void Room_IsAvailableByDefault()
        {
            var room = new Room();
            Assert.That(room.IsAvailable, Is.True);
        }

        [Test]
        public void Room_CanBeSetToUnavailable()
        {
            var room = new Room();
            room.IsAvailable = false;
            Assert.That(room.IsAvailable, Is.False);
        }

        [Test]
        public void Room_CanBeUnavailable_EvenWhenNotFull()
        {
            var room = new Room();
            room.IsAvailable = false;

            Assert.That(room.IsAvailable, Is.False);
            Assert.That(room.IsFull, Is.False);
        }

        [Test]
        public void RoomAssignment_AdmissionDateCannotBeInFuture()
        {
            var room = new Room();
            var patient = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                var assignment = new RoomAssignment(patient, room, DateTime.Now.AddDays(1));
            });
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
        public void Address_CountryCannotBeEmpty()
        {
            var address = new Address();
            Assert.Throws<ArgumentException>(() => address.Country = "");
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
        public void Consultation_RecommendationsCannotBeEmpty()
        {
            var p = new Patient();
            var d = new Doctor();

            var c = new Consultation(p, d, DateTime.Now, "Notes");
            Assert.Throws<ArgumentException>(() => c.Recommendations = "");
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
        public void Surgery_TypeCannotBeEmpty()
        {
            var p = new Patient();
            var sDoc = new SurgeonDoctor();
            var surgery = new Surgery(p, sDoc, DateTime.Now);
            Assert.Throws<ArgumentException>(() => surgery.Type = "");
        }

        [Test]
        public void Surgery_StartTimeIsUnrealistic()
        {
            var p = new Patient();
            var sDoc = new SurgeonDoctor();
            Assert.Throws<ArgumentException>(() =>
            {
                var surgery = new Surgery(p, sDoc, DateTime.Now.AddYears(2));
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

        [Test]
        public void SurgeryStaffParticipation_RoleCannotBeEmpty()
        {
            var ssp = new SurgeryStaffParticipation();
            Assert.Throws<ArgumentException>(() => ssp.Role = "");
        }


  

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
            var room = new Room();
            var patient = new Patient();

            var assignment = new RoomAssignment(patient, room, DateTime.Today);

            Assert.That(room.IsFull, Is.True);
        }

        [Test]
        public void Room_AddAssignmentThrowsWhenFull()
        {
            var room = new Room(); 
            var p1 = new Patient();
            var p2 = new Patient();

            var a1 = new RoomAssignment(p1, room, DateTime.Today);

            Assert.That(room.IsFull, Is.True);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var a2 = new RoomAssignment(p2, room, DateTime.Today);
            });
        }


        

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
            var dischargeDate = new DateTime(2024, 1, 11);

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


     

        [Test]
        public void Patient_ExtentIncreases()
        {
            int before = Patient.Extent.Count;
            var p = new Patient();
            Assert.That(Patient.Extent.Count, Is.EqualTo(before + 1));
            Patient.Extent.Remove(p);
        }

        [Test]
        public void Doctor_ExtentIncreases()
        {
            int before = Doctor.Extent.Count;
            var d = new Doctor();
            Assert.That(Doctor.Extent.Count, Is.EqualTo(before + 1));
            Doctor.Extent.Remove(d);
        }

        [Test]
        public void Appointment_ExtentIncreases()
        {
            int before = Appointment.Extent.Count;
            var a = new Appointment();
            Assert.That(Appointment.Extent.Count, Is.EqualTo(before + 1));
            Appointment.Extent.Remove(a);
        }

        [Test]
        public void Consultation_ExtentIncreases()
        {
            int before = Consultation.Extent.Count;
            var c = new Consultation(new Patient(), new Doctor(), DateTime.Now, "notes");
            Assert.That(Consultation.Extent.Count, Is.EqualTo(before + 1));
            Consultation.Extent.Remove(c);
        }

        [Test]
        public void Department_ExtentIncreases()
        {
            int before = Department.Extent.Count;
            var d = new Department();
            Assert.That(Department.Extent.Count, Is.EqualTo(before + 1));
            Department.Extent.Remove(d);
        }

        [Test]
        public void Diagnosis_ExtentIncreases()
        {
            int before = Diagnosis.Extent.Count;
            var d = new Diagnosis(new Patient(), new Doctor(), "desc", DateTime.Now);
            Assert.That(Diagnosis.Extent.Count, Is.EqualTo(before + 1));
            Diagnosis.Extent.Remove(d);
        }

        [Test]
        public void Nurse_ExtentIncreases()
        {
            int before = Nurse.Extent.Count;
            var n = new Nurse();
            Assert.That(Nurse.Extent.Count, Is.EqualTo(before + 1));
            Nurse.Extent.Remove(n);
        }

        [Test]
        public void Prescription_ExtentIncreases()
        {
            int before = Prescription.Extent.Count;
            var p = new Prescription();
            Assert.That(Prescription.Extent.Count, Is.EqualTo(before + 1));
            Prescription.Extent.Remove(p);
        }

        [Test]
        public void Room_ExtentIncreases()
        {
            int before = Room.Extent.Count;
            var r = new Room();
            Assert.That(Room.Extent.Count, Is.EqualTo(before + 1));
            Room.Extent.Remove(r);
        }

        [Test]
        public void RoomAssignment_ExtentIncreases()
        {
            int before = RoomAssignment.Extent.Count;
            var ra = new RoomAssignment(new Patient(), new Room(), DateTime.Now);
            Assert.That(RoomAssignment.Extent.Count, Is.EqualTo(before + 1));
            RoomAssignment.Extent.Remove(ra);
        }

        [Test]
        public void Surgery_ExtentIncreases()
        {
            int before = Surgery.Extent.Count;
            var s = new Surgery(new Patient(), new SurgeonDoctor(), DateTime.Now);
            Assert.That(Surgery.Extent.Count, Is.EqualTo(before + 1));
            Surgery.Extent.Remove(s);
        }

        [Test]
        public void SurgeryStaffParticipation_ExtentIncreases()
        {
            int before = SurgeryStaffParticipation.Extent.Count;
            var ssp = new SurgeryStaffParticipation();
            Assert.That(SurgeryStaffParticipation.Extent.Count, Is.EqualTo(before + 1));
            SurgeryStaffParticipation.Extent.Remove(ssp);
        }


      

        [Test]
        public void Patient_SaveAndLoadExtent_PreservesSavedCount()
        {
            string path = "patients_test.json";

            
            var p1 = new Patient();
            var p2 = new Patient();
            int savedCount = Patient.Extent.Count;

            Patient.SaveExtent(path);

           
            var p3 = new Patient();
            Assert.That(Patient.Extent.Count, Is.GreaterThan(savedCount));

            
            Patient.LoadExtent(path);

            Assert.That(Patient.Extent.Count, Is.EqualTo(savedCount));

            if (File.Exists(path))
                File.Delete(path);
        }


       

        [Test]
        public void Address_Constructor_SetsDefaultValues()
        {
            var address = new Address();
            Assert.That(address.Country, Is.EqualTo("Unknown"));
            Assert.That(address.City, Is.EqualTo("Unknown"));
            Assert.That(address.Street, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Appointment_Constructor_SetsDefaultValues()
        {
            var appointment = new Appointment();
            Assert.That(appointment.Status, Is.EqualTo("Scheduled"));
           
            Assert.That(appointment.DateTime, Is.GreaterThan(DateTime.Now));
            Assert.That(appointment.DateTime, Is.EqualTo(DateTime.Now.AddHours(1)).Within(TimeSpan.FromSeconds(5)));
        }

        [Test]
        public void Prescription_Constructor_SetsDefaultValues()
        {
            var prescription = new Prescription();
            Assert.That(prescription.Medication, Is.EqualTo("Unknown"));
            Assert.That(prescription.Dosage, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Department_Constructor_AssignsId()
        {
            var department = new Department();
            Assert.That(department.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Consultation_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Consultation(null!, new Doctor(), DateTime.Now, "notes"));
        }

        [Test]
        public void Consultation_Constructor_ThrowsOnNullDoctor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Consultation(new Patient(), null!, DateTime.Now, "notes"));
        }

        [Test]
        public void Diagnosis_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Diagnosis(null!, new Doctor(), "desc", DateTime.Now));
        }

        [Test]
        public void Diagnosis_Constructor_ThrowsOnNullDoctor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Diagnosis(new Patient(), null!, "desc", DateTime.Now));
        }

        [Test]
        public void RoomAssignment_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RoomAssignment(null!, new Room(), DateTime.Now));
        }

        [Test]
        public void RoomAssignment_Constructor_ThrowsOnNullRoom()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RoomAssignment(new Patient(), null!, DateTime.Now));
        }

        [Test]
        public void Surgery_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Surgery(null!, new SurgeonDoctor(), DateTime.Now));
        }

        [Test]
        public void Surgery_Constructor_ThrowsOnNullSurgeon()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Surgery(new Patient(), null!, DateTime.Now));
        }

        [Test]
        public void Room_AddAssignment_ThrowsOnNullAssignment()
        {
            var room = new Room();
            Assert.Throws<ArgumentNullException>(() => room.AddAssignment(null!));
        }
    }
}
