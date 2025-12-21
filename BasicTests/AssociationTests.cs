using NUnit.Framework;
using HospitalSystem;
using System;
using System.Linq;

namespace HospitalSystem.Tests
{
    // These tests verify that all the connections between classes (associations) 
    // are working correctly on both sides (bidirectional).
    [TestFixture]
    public class AssociationTests
    {
        [SetUp]
        public void Setup()
        {
            Patient.Extent.Clear();
            Doctor.Extent.Clear();
            Appointment.Extent.Clear();
            Department.Extent.Clear();
            Room.Extent.Clear();
            Nurse.Extent.Clear();
        }

        // Checking that assigning a doctor to a patient updates the doctor's list too.
        [Test]
        public void PatientDoctor_BidirectionalLink_SetsCorrectly()
        {
            var doctor = new Doctor();
            var patient = new Patient();

            // Act
            patient.ResponsibleDoctor = doctor;

            // Assert
            Assert.That(patient.ResponsibleDoctor, Is.EqualTo(doctor));
            Assert.That(doctor.ResponsibleForPatients, Contains.Item(patient));

            // Act: Change doctor and check if the old link is broken and new one made.
            var doctor2 = new Doctor();
            patient.ResponsibleDoctor = doctor2;

            Assert.That(doctor.ResponsibleForPatients, Does.Not.Contain(patient));
            Assert.That(doctor2.ResponsibleForPatients, Contains.Item(patient));
        }

        // Testing the qualified association: lookup an appointment by its date/time.
        [Test]
        public void DoctorAppointment_QualifiedAssociation_Works()
        {
            var doctor = new Doctor();
            var patient = new Patient();
            var time = new DateTime(2030, 1, 1, 10, 0, 0); // Far future, stable value

            var appointment = new Appointment(patient, doctor, time);

            // Verify lookup by qualifier [date, time]
            Assert.That(doctor.ConductedAppointments.ContainsKey(time), Is.True);
            Assert.That(doctor.ConductedAppointments[time], Is.EqualTo(appointment));
        }

        // Testing the qualified lookup for employees in a department by their ID.
        [Test]
        public void DepartmentEmployee_QualifiedAssociation_Works()
        {
            var department = new Department("General");
            var doctor = new Doctor { NationalID = "DOC123" };

            // Act
            department.AddEmployee(doctor);

            // Assert: Lookup by qualifier [ID]
            var found = department.GetEmployeeById("DOC123");
            Assert.That(found, Is.EqualTo(doctor));
        }

        // Checking the nurse-to-appointment linkage.
        [Test]
        public void NurseAppointment_AssistsLink_IsBidirectional()
        {
            var nurse = new Nurse();
            var appointment = new Appointment(new Patient(), new Doctor(), DateTime.Today.AddDays(1));

            // Act
            appointment.AddAssistingNurse(nurse);

            // Assert
            Assert.That(nurse.AssistedAppointment, Is.EqualTo(appointment));
            Assert.That(appointment.AssistingNurses, Contains.Item(nurse));

            // Act: Try to add to another appointment - should fail since nurse is busy.
            var appointment2 = new Appointment(new Patient(), new Doctor(), DateTime.Today.AddDays(2));
            Assert.Throws<InvalidOperationException>(() => appointment2.AddAssistingNurse(nurse));

            // Act: Remove and check if link is cleared.
            appointment.RemoveAssistingNurse(nurse);
            Assert.That(nurse.AssistedAppointment, Is.Null);
            Assert.That(appointment.AssistingNurses, Does.Not.Contain(nurse));
        }

        // Testing department head logic and bidirectional connection.
        [Test]
        public void DepartmentDoctor_HeadOfLink_IsBidirectional()
        {
            var department1 = new Department("Cardiology");
            var head = new PermanentDoctor();
            department1.AddEmployee(head);

            // Act
            department1.SetHead(head);

            // Assert
            Assert.That(department1.Head, Is.EqualTo(head));
            Assert.That(head.HeadedDepartment, Is.EqualTo(department1));

            // Act: Move doctor to another department.
            var department2 = new Department("Neurology");
            department1.RemoveEmployee(head);
            department2.AddEmployee(head);

            // Should fail because they are still marked as head of the first department.
            Assert.Throws<InvalidOperationException>(() => department2.SetHead(head));
        }

        // Checking 1:1 composition reverse connection for Person and Address.
        [Test]
        public void PersonAddress_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var address = new Address();

            // Act
            patient.Address = address;

            // Assert
            Assert.That(patient.Address, Is.EqualTo(address));
            Assert.That(address.Person, Is.EqualTo(patient));

            // Change address and check if old one is unlinked.
            var address2 = new Address();
            patient.Address = address2;

            Assert.That(address.Person, Is.Null);
            Assert.That(address2.Person, Is.EqualTo(patient));
        }

        // Testing the composition logic for Rooms in a Department.
        [Test]
        public void DepartmentRoom_Composition_RemovesFromGlobalExtent()
        {
            var department = new Department("ICU");
            var room = new Room { RoomNumber = "101" };
            department.AddRoom(room);

            Assert.That(Room.Extent, Contains.Item(room));

            // Act: Remove from department - should be deleted from system entirely.
            department.RemoveRoom(room);

            Assert.That(Room.Extent, Does.Not.Contain(room));
            Assert.That(room.Department, Is.Null);
        }

        // Diagnostic test to ensure dictionary isn't shared across instances.
        [Test]
        public void ConductedAppointments_IsPerInstance()
        {
            var doctor1 = new Doctor();
            var doctor2 = new Doctor();
            var patient = new Patient();
            var time = new DateTime(2030, 5, 5, 10, 0, 0);

            var appointment = new Appointment(patient, doctor1, time);

            Assert.That(doctor1.ConductedAppointments.Count, Is.EqualTo(1));
            Assert.That(doctor2.ConductedAppointments.Count, Is.EqualTo(0));
        }

        // Bidirectional checks for record types and the history hub.
        [Test]
        public void ConsultationMedicalRecord_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var record = patient.MedicalRecord;
            var consultation = new Consultation(record, DateTime.Today, "Notes");
            Assert.That(record.Consultations, Contains.Item(consultation));
            Assert.That(consultation.MedicalRecord, Is.EqualTo(record));
        }

        [Test]
        public void DiagnosisMedicalRecord_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var record = patient.MedicalRecord;
            var diagnosis = new Diagnosis(record, "Flu", DateTime.Today);
            Assert.That(record.Diagnoses, Contains.Item(diagnosis));
            Assert.That(diagnosis.MedicalRecord, Is.EqualTo(record));
        }

        [Test]
        public void PrescriptionMedicalRecord_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var record = patient.MedicalRecord;
            var prescription = new Prescription(record);
            Assert.That(record.Prescriptions, Contains.Item(prescription));
            Assert.That(prescription.MedicalRecord, Is.EqualTo(record));
        }

        // Comprehensive test for completing an appointment and generating all result objects.
        [Test]
        public void Appointment_CompleteFlow_CreatesResults()
        {
            var doctor = new Doctor();
            var patient = new Patient();
            var time = DateTime.Today.AddDays(1);
            var appointment = new Appointment(patient, doctor, time);

            // Act
            appointment.CompleteAppointment("Discussed symptoms", "Common Flu", "Paracetamol", "500mg");

            // Assert everything is created and linked back correctly.
            Assert.That(appointment.Status, Is.EqualTo("Completed"));
            Assert.That(appointment.ResultingConsultation, Is.Not.Null);
            Assert.That(appointment.ResultingConsultation!.SourceAppointment, Is.EqualTo(appointment));
            Assert.That(patient.MedicalRecord.Consultations, Contains.Item(appointment.ResultingConsultation));

            Assert.That(appointment.Diagnosis, Is.Not.Null);
            Assert.That(appointment.Diagnosis!.SourceAppointment, Is.EqualTo(appointment));
            Assert.That(patient.MedicalRecord.Diagnoses, Contains.Item(appointment.Diagnosis));

            Assert.That(appointment.Prescription, Is.Not.Null);
            Assert.That(appointment.Prescription!.SourceAppointment, Is.EqualTo(appointment));
            Assert.That(patient.MedicalRecord.Prescriptions, Contains.Item(appointment.Prescription));

            // Verify the nested results link to the consultation.
            Assert.That(appointment.ResultingConsultation!.Diagnoses, Contains.Item(appointment.Diagnosis));
            Assert.That(appointment.ResultingConsultation!.Prescriptions, Contains.Item(appointment.Prescription));
            Assert.That(appointment.Diagnosis.Consultation, Is.EqualTo(appointment.ResultingConsultation));
            Assert.That(appointment.Prescription.Consultation, Is.EqualTo(appointment.ResultingConsultation));
        }

        // Basic department bidirectional check.
        [Test]
        public void EmployeeDepartment_BidirectionalLink_Works()
        {
            var department = new Department("Cardiology");
            var doctor = new Doctor();
            doctor.Department = department;
            Assert.That(department.Employees, Contains.Item(doctor));
            Assert.That(doctor.Department, Is.EqualTo(department));
            doctor.Department = null;
            Assert.That(department.Employees, Does.Not.Contain(doctor));
        }

        [Test]
        public void RoomDepartment_BidirectionalLink_Works()
        {
            var department = new Department("Neurology");
            var room = new Room();
            room.Department = department;
            Assert.That(department.Rooms, Contains.Item(room));
            Assert.That(room.Department, Is.EqualTo(department));
        }

        // Checking the staff participation association class link.
        [Test]
        public void SurgeryStaff_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var surgeon = new Doctor { SurgeonSpeciality = "Orthopedic" };
            var surgery = new Surgery(patient, surgeon, DateTime.Now.AddDays(1));
            var nurse = new Nurse { Name = "Assisting Nurse" };

            // Act
            var participation = new SurgeryStaffParticipation(surgery, nurse, "Assistant");

            // Assert
            Assert.That(surgery.Staff, Contains.Item(participation));
            Assert.That(nurse.SurgeryParticipations, Contains.Item(participation));
            Assert.That(participation.Surgery, Is.EqualTo(surgery));
            Assert.That(participation.StaffMember, Is.EqualTo(nurse));
        }
    }
}