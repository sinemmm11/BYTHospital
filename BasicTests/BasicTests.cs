using NUnit.Framework;
using System;

namespace HospitalSystem.Tests
{
    public class BasicTests
    {
        
        private static Department CreateDepartment(string name = "Cardiology")
        {
            return new Department(name) { Location = "Floor 1" };
        }

        private static Person CreateDoctor(Department dept, bool permanent = true, DoctorRole roles = DoctorRole.Consultant | DoctorRole.Surgeon)
        {
            var d = new Person
            {
                Name = "Doc",
                Surname = "One",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "F",
                PhoneNumber = "123",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            d.MakeEmployee(EmployeeType.Doctor, dept, salary: 10000);
            d.SetDoctorRoles(roles);

            if (permanent)
                d.SetDoctorEmployment(EmploymentType.Permanent, DateTime.Today.AddYears(-2), null);
            else
                d.SetDoctorEmployment(EmploymentType.Contractor, DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(3));

            d.Specialization = "General";
            d.LicenseNumber = "LIC-1";

            if (d.DoctorRoles.HasFlag(DoctorRole.Consultant))
                d.ConsultingHours = "09:00-12:00";

            if (d.DoctorRoles.HasFlag(DoctorRole.Surgeon))
                d.SurgeonSpeciality = "General Surgery";

            return d;
        }

        private static Person CreateNurse(Department dept)
        {
            var n = new Person
            {
                Name = "Nurse",
                Surname = "One",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "M",
                PhoneNumber = "555",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            n.MakeEmployee(EmployeeType.Nurse, dept, salary: 6000);
            n.RegistrationNumber = "REG-1";
            n.ShiftDetails = "Night";

            return n;
        }

        private static Person CreatePatient(Person responsibleDoctor)
        {
            var p = new Person
            {
                Name = "Pat",
                Surname = "One",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "F",
                PhoneNumber = "999",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            p.MakePatient(responsibleDoctor);
            return p;
        }

       
        [SetUp]
        public void CommonSetup()
        {
            Appointment.Extent.Clear();
            Department.Extent.Clear();
            Room.Extent.Clear();
            Surgery.Extent.Clear();
            Diagnosis.Extent.Clear();
            Prescription.Extent.Clear();
            RoomAssignment.Extent.Clear();
            SurgeryStaffParticipation.Extent.Clear();
            Consultation.Extent.Clear();
        }

     

        [Test]
        public void Person_NameCannotBeEmpty()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.Name = "");
        }

        [Test]
        public void Person_SurnameCannotBeEmpty()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.Surname = "");
        }

        [Test]
        public void Person_NationalIdCannotBeEmpty()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.NationalID = "");
        }

        [Test]
        public void Person_GenderCannotBeEmpty()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.Gender = "");
        }

        [Test]
        public void Person_PhoneNumberCannotBeEmpty()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.PhoneNumber = "");
        }

        [Test]
        public void Person_DateOfBirthCannotBeInFuture()
        {
            var p = new Person();
            Assert.Throws<ArgumentException>(() => p.DateOfBirth = DateTime.Now.AddDays(1));
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
        public void Person_Address_Sets_BackReference()
        {
            var p = new Person
            {
                Name = "A",
                Surname = "B",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "F",
                PhoneNumber = "111",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var newAddr = new Address { Country = "PL", City = "Warsaw", Street = "Main" };
            p.Address = newAddr;

            Assert.That(newAddr.Person, Is.EqualTo(p));
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
        public void Department_AddEmployee_SetsBidirectionalRelation()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);

            Assert.That(dep.Employees, Does.Contain(doc));
            Assert.That(doc.Department, Is.EqualTo(dep));
            Assert.That(dep.GetEmployeeById(doc.NationalID), Is.EqualTo(doc));
        }

        [Test]
        public void Department_AddEmployee_Throws_When_NotEmployee()
        {
            var dep = CreateDepartment();

            var p = new Person
            {
                Name = "X",
                Surname = "Y",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "F",
                PhoneNumber = "111",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Assert.Throws<ArgumentException>(() => dep.AddEmployee(p));
        }

        [Test]
        public void Department_SetHead_Works_For_Permanent_Doctor_In_Department()
        {
            var dep = CreateDepartment();
            var head = CreateDoctor(dep, permanent: true);

            dep.SetHead(head);

            Assert.That(dep.Head, Is.EqualTo(head));
        }

        [Test]
        public void Department_SetHead_Throws_For_Contractor_Doctor()
        {
            var dep = CreateDepartment();
            var contractor = CreateDoctor(dep, permanent: false);

            Assert.Throws<ArgumentException>(() => dep.SetHead(contractor));
        }

      

        [Test]
        public void MakePatient_Throws_If_Responsible_IsNotDoctor()
        {
            var notDoctor = new Person
            {
                Name = "X",
                Surname = "Y",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "F",
                PhoneNumber = "111",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            var p = new Person
            {
                Name = "P",
                Surname = "Q",
                NationalID = Guid.NewGuid().ToString(),
                Gender = "M",
                PhoneNumber = "222",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            Assert.Throws<ArgumentException>(() => p.MakePatient(notDoctor));
        }

        [Test]
        public void MakePatient_Sets_MedicalRecord_And_ResponsibleDoctor()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            Assert.That(patient.IsPatient, Is.True);
            Assert.That(patient.MedicalRecord, Is.Not.Null);
            Assert.That(patient.ResponsibleDoctor, Is.EqualTo(doc));
            Assert.That(doc.ResponsibleForPatients, Does.Contain(patient));
        }

      
        [Test]
        public void Appointment_CannotBeInPast()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Appointment(patient, doc, DateTime.Now.AddMinutes(-5));
            });
        }

        [Test]
        public void Doctor_ThrowsOnDuplicateAppointment_Time()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var p1 = CreatePatient(doc);
            var p2 = CreatePatient(doc);

            var t = DateTime.Now.AddHours(3);
            _ = new Appointment(p1, doc, t);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = new Appointment(p2, doc, t);
            });
        }

        [Test]
        public void Appointment_Allow_Assisting_Nurse()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var nurse = CreateNurse(dep);
            var patient = CreatePatient(doc);

            var appt = new Appointment(patient, doc, DateTime.Now.AddHours(2));
            appt.AddAssistingNurse(nurse);

            Assert.That(appt.AssistingNurses, Does.Contain(nurse));
        }

       

        [Test]
        public void Consultation_NotesCannotBeEmpty()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Consultation(patient.MedicalRecord!, DateTime.Now, "");
            });
        }

        [Test]
        public void Diagnosis_DescriptionCannotBeEmpty()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Diagnosis(patient.MedicalRecord!, "", DateTime.Now);
            });
        }

        [Test]
        public void Diagnosis_DateCannotBeInFuture()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Diagnosis(patient.MedicalRecord!, "Flu", DateTime.Now.AddDays(1));
            });
        }

        [Test]
        public void Prescription_MedicationCannotBeEmpty()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var pr = new Prescription(patient.MedicalRecord!);
            Assert.Throws<ArgumentException>(() => pr.Medication = "");
        }

        [Test]
        public void Prescription_DosageCannotBeEmpty()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var pr = new Prescription(patient.MedicalRecord!);
            Assert.Throws<ArgumentException>(() => pr.Dosage = "");
        }

       
        [Test]
        public void Room_CapacityMustBeGreaterThanZero()
        {
            var r = new Room();
            Assert.Throws<ArgumentOutOfRangeException>(() => r.Capacity = 0);
        }

        [Test]
        public void Room_IsFull_WhenAssignmentsReachCapacity()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var room = new Room { Capacity = 1, RoomNumber = "101", Type = "Standard" };
            _ = new RoomAssignment(patient, room, DateTime.Now.AddDays(-1));

            Assert.That(room.IsFull, Is.True);
            Assert.That(room.IsAvailable, Is.False);
        }

        [Test]
        public void RoomAssignment_CannotBeInFuture()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var room = new Room();

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new RoomAssignment(patient, room, DateTime.Now.AddDays(1));
            });
        }

        [Test]
        public void RoomAssignment_DischargeCannotBeBeforeAdmission()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var room = new Room();
            var admission = DateTime.Now.AddDays(-2);

            var assignment = new RoomAssignment(patient, room, admission);

            Assert.Throws<ArgumentException>(() =>
            {
                assignment.Discharge(admission.AddDays(-1));
            });
        }

        [Test]
        public void Patient_CannotBeAdmittedTwice()
        {
            var dep = CreateDepartment();
            var doc = CreateDoctor(dep);
            var patient = CreatePatient(doc);

            var room1 = new Room { Capacity = 1 };
            var room2 = new Room { Capacity = 1 };

            _ = new RoomAssignment(patient, room1, DateTime.Now.AddDays(-1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = new RoomAssignment(patient, room2, DateTime.Now.AddDays(-1));
            });
        }

       

        [Test]
        public void Surgery_Throws_If_Doctor_HasNoSurgeonRole()
        {
            var dep = CreateDepartment();
            var consultantOnly = CreateDoctor(dep, permanent: true, roles: DoctorRole.Consultant);
            var patient = CreatePatient(CreateDoctor(dep));

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Surgery(patient, consultantOnly, DateTime.Now.AddDays(1));
            });
        }
    }
}
