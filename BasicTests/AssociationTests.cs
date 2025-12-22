using NUnit.Framework;
using HospitalSystem;
using System;
using System.Linq;

namespace HospitalSystem.Tests
{
   
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

       
        [Test]
        public void PatientDoctor_BidirectionalLink_SetsCorrectly()
        {
            var doctor = new Doctor();
            var patient = new Patient();

            
            patient.ResponsibleDoctor = doctor;

           
            Assert.That(patient.ResponsibleDoctor, Is.EqualTo(doctor));
            Assert.That(doctor.ResponsibleForPatients, Contains.Item(patient));

            
            var doctor2 = new Doctor();
            patient.ResponsibleDoctor = doctor2;

            Assert.That(doctor.ResponsibleForPatients, Does.Not.Contain(patient));
            Assert.That(doctor2.ResponsibleForPatients, Contains.Item(patient));
        }

        
        [Test]
        public void DoctorAppointment_QualifiedAssociation_Works()
        {
            var doctor = new Doctor();
            var patient = new Patient();
            var time = new DateTime(2030, 1, 1, 10, 0, 0); 

            var appointment = new Appointment(patient, doctor, time);

            
            Assert.That(doctor.ConductedAppointments.ContainsKey(time), Is.True);
            Assert.That(doctor.ConductedAppointments[time], Is.EqualTo(appointment));
        }

       
        [Test]
        public void DepartmentEmployee_QualifiedAssociation_Works()
        {
            var department = new Department("General");
            var doctor = new Doctor { NationalID = "DOC123" };

            
            department.AddEmployee(doctor);

            
            var found = department.GetEmployeeById("DOC123");
            Assert.That(found, Is.EqualTo(doctor));
        }

       
        [Test]
        public void NurseAppointment_AssistsLink_IsBidirectional()
        {
            var nurse = new Nurse();
            var appointment = new Appointment(new Patient(), new Doctor(), DateTime.Today.AddDays(1));

            
            appointment.AddAssistingNurse(nurse);

            
            Assert.That(nurse.AssistedAppointment, Is.EqualTo(appointment));
            Assert.That(appointment.AssistingNurses, Contains.Item(nurse));

           
            var appointment2 = new Appointment(new Patient(), new Doctor(), DateTime.Today.AddDays(2));
            Assert.Throws<InvalidOperationException>(() => appointment2.AddAssistingNurse(nurse));

            
            appointment.RemoveAssistingNurse(nurse);
            Assert.That(nurse.AssistedAppointment, Is.Null);
            Assert.That(appointment.AssistingNurses, Does.Not.Contain(nurse));
        }

        
        [Test]
        public void DepartmentDoctor_HeadOfLink_IsBidirectional()
        {
            var department1 = new Department("Cardiology");
            var head = new PermanentDoctor();
            department1.AddEmployee(head);

            
            department1.SetHead(head);

            
            Assert.That(department1.Head, Is.EqualTo(head));
            Assert.That(head.HeadedDepartment, Is.EqualTo(department1));

           
            var department2 = new Department("Neurology");
            department1.RemoveEmployee(head);
            department2.AddEmployee(head);

           
            Assert.Throws<InvalidOperationException>(() => department2.SetHead(head));
        }

       
        [Test]
        public void PersonAddress_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var address = new Address();

           
            patient.Address = address;

           
            Assert.That(patient.Address, Is.EqualTo(address));
            Assert.That(address.Person, Is.EqualTo(patient));

           
            var address2 = new Address();
            patient.Address = address2;

            Assert.That(address.Person, Is.Null);
            Assert.That(address2.Person, Is.EqualTo(patient));
        }

       
        [Test]
        public void DepartmentRoom_Composition_RemovesFromGlobalExtent()
        {
            var department = new Department("ICU");
            var room = new Room { RoomNumber = "101" };
            department.AddRoom(room);

            Assert.That(Room.Extent, Contains.Item(room));

           
            department.RemoveRoom(room);

            Assert.That(Room.Extent, Does.Not.Contain(room));
            Assert.That(room.Department, Is.Null);
        }

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

        [Test]
        public void Appointment_CompleteFlow_CreatesResults()
        {
            var doctor = new Doctor();
            var patient = new Patient();
            var time = DateTime.Today.AddDays(1);
            var appointment = new Appointment(patient, doctor, time);

            appointment.CompleteAppointment("Discussed symptoms", "Common Flu", "Paracetamol", "500mg");

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

            Assert.That(appointment.ResultingConsultation!.Diagnoses, Contains.Item(appointment.Diagnosis));
            Assert.That(appointment.ResultingConsultation!.Prescriptions, Contains.Item(appointment.Prescription));
            Assert.That(appointment.Diagnosis.Consultation, Is.EqualTo(appointment.ResultingConsultation));
            Assert.That(appointment.Prescription.Consultation, Is.EqualTo(appointment.ResultingConsultation));
        }

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

        [Test]
        public void SurgeryStaff_BidirectionalLink_Works()
        {
            var patient = new Patient();
            var surgeon = new Doctor { SurgeonSpeciality = "Orthopedic" };
            var surgery = new Surgery(patient, surgeon, DateTime.Now.AddDays(1));
            var nurse = new Nurse { Name = "Assisting Nurse" };

            var participation = new SurgeryStaffParticipation(surgery, nurse, "Assistant");

            Assert.That(surgery.Staff, Contains.Item(participation));
            Assert.That(nurse.SurgeryParticipations, Contains.Item(participation));
            Assert.That(participation.Surgery, Is.EqualTo(surgery));
            Assert.That(participation.StaffMember, Is.EqualTo(nurse));
        }
    }
}
