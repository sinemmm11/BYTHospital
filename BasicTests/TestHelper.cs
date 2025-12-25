using System;

namespace HospitalSystem.Tests
{
    public static class TestHelper
    {
        public static Department CreateDepartment(string name = "Cardiology")
        {
            return new Department(name) { Location = "Floor 1" };
        }

        public static Person CreateDoctor(Department dept)
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
            d.SetDoctorRoles(DoctorRole.Consultant | DoctorRole.Surgeon);
            d.SetDoctorEmployment(EmploymentType.Permanent, DateTime.Today.AddYears(-2), null);

            d.Specialization = "General";
            d.LicenseNumber = "LIC-1";
            d.ConsultingHours = "09:00-12:00";
            d.SurgeonSpeciality = "General Surgery";

            return d;
        }

        public static Person CreateNurse(Department dept)
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

        public static Person CreatePatient(Person responsibleDoctor)
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

            p.MakePatient(responsibleDoctor); // MedicalRecord + ResponsibleDoctor bağlar
            return p;
        }
    }
}