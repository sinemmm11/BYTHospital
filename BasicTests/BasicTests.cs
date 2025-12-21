using NUnit.Framework;
using HospitalSystem;
using System;
using System.IO;
using System.Linq;

namespace HospitalSystem.Tests
{
    // This class handles the basic validation and property logic for individual entities.
    public class BasicTests
    {
        // Cleaning up state before each test to ensure a fresh start.
        [SetUp]
        public void CommonSetup()
        {
            Patient.Extent.Clear();
            Doctor.Extent.Clear();
            Appointment.Extent.Clear();
            Department.Extent.Clear();
            Room.Extent.Clear();
            Nurse.Extent.Clear();
            Surgery.Extent.Clear();
            Diagnosis.Extent.Clear();
            Prescription.Extent.Clear();
            RoomAssignment.Extent.Clear();
            SurgeryStaffParticipation.Extent.Clear();
        }

        // --- PERSON & PATIENT VALIDATION ---

        // Checking that basic identity fields can't be empty.
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

        // Birth dates should obviously not be in the future.
        [Test]
        public void Patient_BirthDateCannotBeInFuture()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                p.BirthDate = DateTime.Today.AddDays(1);
            });
        }

        // Middle name is optional but shouldn't just be whitespace.
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

        // Verifying that the allergy list grows and blocks duplicates.
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

        // Making sure the Age property correctly reflects the birthdate.
        [Test]
        public void Patient_Age_IsCorrect()
        {
            var p = new Patient();
            p.BirthDate = new DateTime(2000, 1, 1);
            int expected = DateTime.Today.Year - 2000;
            if (new DateTime(DateTime.Today.Year, 1, 1) > DateTime.Today)
                expected--;
            Assert.That(p.Age, Is.EqualTo(expected));
        }

        // --- DOCTOR & EMPLOYEE VALIDATION ---

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
            var cd = new Doctor();
            Assert.Throws<ArgumentException>(() => cd.ConsultingHours = "");
        }

        [Test]
        public void SurgeonDoctor_SurgeonSpecialityCannotBeEmpty()
        {
            var sd = new Doctor();
            Assert.Throws<ArgumentException>(() => sd.SurgeonSpeciality = "");
        }

        // Ensuring contract/employment ranges make sense.
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

        // Testing the reflexive supervision link between doctors.
        [Test]
        public void Doctor_Supervision_SetsBidirectionalRelation()
        {
            var supervisor = new Doctor();
            var supervised = new Doctor();
            supervisor.AddSupervisedDoctor(supervised);
            Assert.That(supervisor.SupervisedDoctors, Contains.Item(supervised));
            Assert.That(supervised.SupervisingDoctor, Is.EqualTo(supervisor));
        }

        [Test]
        public void Doctor_CannotSuperviseSelf()
        {
            var doctor = new Doctor();
            Assert.Throws<ArgumentException>(() => doctor.AddSupervisedDoctor(doctor));
        }

        [Test]
        public void Doctor_CannotBeSupervisedByMultipleDoctors()
        {
            var supervisor1 = new Doctor();
            var supervisor2 = new Doctor();
            var supervised = new Doctor();
            supervisor1.AddSupervisedDoctor(supervised);
            Assert.Throws<InvalidOperationException>(() => supervisor2.AddSupervisedDoctor(supervised));
        }

        // --- NURSE VALIDATION ---

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

        // --- DEPARTMENT VALIDATION ---

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

        // Making sure employees get linked to the department on both ends.
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

        // Checking the department head role assignment.
        [Test]
        public void Department_SetHead_SetsCorrectly()
        {
            var dep = new Department("Cardiology");
            var head = new PermanentDoctor();
            dep.AddDoctor(head);
            dep.SetHead(head);
            Assert.That(dep.Head, Is.EqualTo(head));
        }

        [Test]
        public void Department_SetHead_ThrowsForNonEmployee()
        {
            var dep = new Department("Cardiology");
            var head = new PermanentDoctor();
            Assert.Throws<ArgumentException>(() => dep.SetHead(head));
        }

        // --- APPOINTMENT VALIDATION ---

        [Test]
        public void Appointment_CannotBeInPast()
        {
            var a = new Appointment(new Patient(), new Doctor(), DateTime.Now.AddHours(1));
            Assert.Throws<ArgumentException>(() => a.DateTime = DateTime.Now.AddMinutes(-1));
        }

        [Test]
        public void Appointment_StatusCannotBeEmpty()
        {
            var a = new Appointment(new Patient(), new Doctor(), DateTime.Now.AddHours(1));
            Assert.Throws<ArgumentException>(() => a.Status = "");
        }

        [Test]
        public void Appointment_Constructor_SetsDefaultValues()
        {
            var appointment = new Appointment(new Patient(), new Doctor(), DateTime.Now.AddHours(1));
            Assert.That(appointment.Status, Is.EqualTo("Scheduled"));
            Assert.That(appointment.DateTime, Is.GreaterThan(DateTime.Now));
        }

        [Test]
        public void Appointment_LinksPatientAndDoctor()
        {
            var p = new Patient();
            var d = new Doctor();
            var a = new Appointment(p, d, DateTime.Now.AddHours(1));
            Assert.That(a.Patient, Is.EqualTo(p));
            Assert.That(a.Doctor, Is.EqualTo(d));
        }

        // Double booking a doctor is a big no-no.
        [Test]
        public void Doctor_ThrowsOnDuplicateAppointment()
        {
            var doctor = new Doctor();
            var patient1 = new Patient();
            var patient2 = new Patient();
            var appointmentTime = DateTime.Now.AddDays(1);
            var appointment1 = new Appointment(patient1, doctor, appointmentTime); 
            Assert.That(doctor.ConductedAppointments.Values.Contains(appointment1), Is.True);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var appointment2 = new Appointment(patient2, doctor, appointmentTime);
            });
        }

        // Checking that appointments come out sorted by date.
        [Test]
        public void Doctor_GetAppointmentsChronologically_ReturnsSorted()
        {
            var doctor = new Doctor();
            var patient = new Patient();
            var time = new DateTime(2030, 1, 1, 10, 0, 0);
            var appointment2 = new Appointment(patient, doctor, time.AddHours(2));
            var appointment1 = new Appointment(patient, doctor, time.AddHours(1));
            var appointment3 = new Appointment(patient, doctor, time.AddHours(3));
            var sortedAppointments = doctor.GetAppointmentsChronologically().ToList();
            Assert.That(sortedAppointments[0], Is.EqualTo(appointment1));
            Assert.That(sortedAppointments[1], Is.EqualTo(appointment2));
            Assert.That(sortedAppointments[2], Is.EqualTo(appointment3));
        }

        [Test]
        public void Appointment_CanHaveDiagnosisAndPrescription()
        {
            var appointment = new Appointment(new Patient(), new Doctor(), DateTime.Now.AddHours(1));
            var diagnosis = new Diagnosis(appointment.Patient.MedicalRecord, "Flu", DateTime.Now);
            var prescription = new Prescription(appointment.Patient.MedicalRecord);
            appointment.Diagnosis = diagnosis;
            appointment.Prescription = prescription;
            Assert.That(appointment.Diagnosis, Is.EqualTo(diagnosis));
            Assert.That(appointment.Prescription, Is.EqualTo(prescription));
        }

        // --- ROOM & ASSIGNMENT VALIDATION ---

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
            room.SetOutOfService(true);
            Assert.That(room.IsAvailable, Is.False);
        }

        [Test]
        public void Room_CanBeUnavailable_EvenWhenNotFull()
        {
            var room = new Room();
            room.SetOutOfService(true);
            Assert.That(room.IsAvailable, Is.False);
            Assert.That(room.IsFull, Is.False);
        }

        [Test]
        public void Room_IsFull_WhenAssignmentsReachCapacity()
        {
            var room = new Room();
            var patient = new Patient();
            _ = new RoomAssignment(patient, room, DateTime.Today);
            Assert.That(room.IsFull, Is.True);
        }

        [Test]
        public void Room_AddAssignmentThrowsWhenFull()
        {
            var room = new Room();
            var p1 = new Patient();
            var p2 = new Patient();
            _ = new RoomAssignment(p1, room, DateTime.Today);
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = new RoomAssignment(p2, room, DateTime.Today);
            });
        }

        [Test]
        public void Room_AddAssignment_ThrowsOnNullAssignment()
        {
            var room = new Room();
            Assert.Throws<ArgumentNullException>(() => room.AddAssignment(null!));
        }

        [Test]
        public void RoomAssignment_AdmissionDateCannotBeInFuture()
        {
            var room = new Room();
            var patient = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new RoomAssignment(patient, room, DateTime.Now.AddDays(1));
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

        // Multiplicity XOR constraint: A patient can't be in surgery and in a room at once.
        [Test]
        public void Patient_CannotBeAdmittedDuringActiveSurgery()
        {
            var patient = new Patient();
            var surgeon = new Doctor { SurgeonSpeciality = "General Surgery" };
            var surgery = new Surgery(patient, surgeon, DateTime.Now);
            Assert.That(patient.HasActiveSurgery(), Is.True);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var room = new Room();
                var roomAssignment = new RoomAssignment(patient, room, DateTime.Now);
            });
        }

        [Test]
        public void Patient_CannotHaveSurgeryWhileAdmitted()
        {
            var patient = new Patient();
            var room = new Room();
            var roomAssignment = new RoomAssignment(patient, room, DateTime.Now);
            Assert.That(patient.HasActiveRoomAssignment(), Is.True);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var surgeon = new Doctor { SurgeonSpeciality = "General Surgery" };
                var surgery = new Surgery(patient, surgeon, DateTime.Now);
            });
        }

        [Test]
        public void Patient_CannotBeAdmittedTwice()
        {
            var patient = new Patient();
            var room1 = new Room();
            var roomAssignment1 = new RoomAssignment(patient, room1, DateTime.Now);
            Assert.That(patient.HasActiveRoomAssignment(), Is.True);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var room2 = new Room();
                var roomAssignment2 = new RoomAssignment(patient, room2, DateTime.Now);
            });
        }

        // --- RECORD TYPE VALIDATION ---

        [Test]
        public void Prescription_MedicationCannotBeEmpty()
        {
            var pr = new Prescription(new Patient().MedicalRecord);
            Assert.Throws<ArgumentException>(() => pr.Medication = "");
        }

        [Test]
        public void Prescription_DosageCannotBeEmpty()
        {
            var pr = new Prescription(new Patient().MedicalRecord);
            Assert.Throws<ArgumentException>(() => pr.Dosage = "");
        }

        [Test]
        public void Prescription_Instructions_CanBeNull()
        {
            var pr = new Prescription(new Patient().MedicalRecord);
            pr.Instructions = null;
            Assert.That(pr.Instructions, Is.Null);
        }

        [Test]
        public void Prescription_Constructor_SetsDefaultValues()
        {
            var prescription = new Prescription(new Patient().MedicalRecord);
            Assert.That(prescription.Medication, Is.EqualTo("Unknown"));
            Assert.That(prescription.Dosage, Is.EqualTo("Unknown"));
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
        public void Address_Constructor_SetsDefaultValues()
        {
            var address = new Address();
            Assert.That(address.Country, Is.EqualTo("Unknown"));
            Assert.That(address.City, Is.EqualTo("Unknown"));
            Assert.That(address.Street, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Diagnosis_DescriptionCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Diagnosis(p.MedicalRecord, "", DateTime.Today);
            });
        }

        [Test]
        public void Diagnosis_DateCannotBeInFuture()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Diagnosis(p.MedicalRecord, "Flu", DateTime.Today.AddDays(1));
            });
        }

        [Test]
        public void Diagnosis_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Diagnosis(null!, "desc", DateTime.Now));
        }

        [Test]
        public void Consultation_NotesCannotBeEmpty()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Consultation(p.MedicalRecord, DateTime.Now, "");
            });
        }

        [Test]
        public void Consultation_RecommendationsCannotBeEmpty()
        {
            var p = new Patient();
            var c = new Consultation(p.MedicalRecord, DateTime.Now, "Notes");
            Assert.Throws<ArgumentException>(() => c.Recommendations = "");
        }

        [Test]
        public void Consultation_DateTooFarInFutureThrows()
        {
            var p = new Patient();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Consultation(p.MedicalRecord, DateTime.Now.AddDays(2), "Check-up");
            });
        }

        [Test]
        public void Consultation_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Consultation(null!, DateTime.Now, "notes"));
        }

        // --- SURGERY VALIDATION ---

        [Test]
        public void Surgery_TypeCannotBeEmpty()
        {
            var p = new Patient();
            var sDoc = new Doctor { SurgeonSpeciality = "General Surgery" };
            var surgery = new Surgery(p, sDoc, DateTime.Now);
            Assert.Throws<ArgumentException>(() => surgery.Type = "");
        }

        [Test]
        public void Surgery_StartTimeIsUnrealistic()
        {
            var p = new Patient();
            var sDoc = new Doctor { SurgeonSpeciality = "General Surgery" };
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Surgery(p, sDoc, DateTime.Now.AddYears(2));
            });
        }

        [Test]
        public void Surgery_EndTime_IsDerivedFromDuration()
        {
            var p = new Patient();
            var sDoc = new Doctor { SurgeonSpeciality = "General Surgery" };
            var start = DateTime.Now.AddDays(1);
            var surgery = new Surgery(p, sDoc, start);
            Assert.That(surgery.EndTime, Is.Null);
            var duration = TimeSpan.FromMinutes(90);
            surgery.Duration = duration;
            Assert.That(surgery.EndTime, Is.EqualTo(start.Add(duration)));
        }

        [Test]
        public void Surgery_Constructor_ThrowsOnNullPatient()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Surgery(null!, new Doctor { SurgeonSpeciality = "General Surgery" }, DateTime.Now));
        }

        [Test]
        public void Surgery_Constructor_ThrowsOnNullSurgeon()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Surgery(new Patient(), null!, DateTime.Now));
        }

        [Test]
        public void SurgeryStaffParticipation_RoleCannotBeEmpty()
        {
            var ssp = new SurgeryStaffParticipation();
            Assert.Throws<ArgumentException>(() => ssp.Role = "");
        }

        // --- EXTENTS ---

        // Checking that static extents correctly track new instances.
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
            var a = new Appointment(new Patient(), new Doctor(), DateTime.Now.AddHours(1));
            Assert.That(Appointment.Extent.Count, Is.EqualTo(before + 1));
            Appointment.Extent.Remove(a);
        }

        [Test]
        public void Consultation_ExtentIncreases()
        {
            int before = Consultation.Extent.Count;
            var c = new Consultation(new Patient().MedicalRecord, DateTime.Now, "notes");
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
            var d = new Diagnosis(new Patient().MedicalRecord, "desc", DateTime.Now);
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
            var p = new Prescription(new Patient().MedicalRecord);
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
            var s = new Surgery(new Patient(), new Doctor { SurgeonSpeciality = "General Surgery" }, DateTime.Now);
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

        // --- PERSISTENCE ---

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
            Patient.Extent.Remove(p1);
            Patient.Extent.Remove(p2);
            Patient.Extent.Remove(p3);
        }
    }
}
