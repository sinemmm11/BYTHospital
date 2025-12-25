using System;
using NUnit.Framework;

namespace HospitalSystem.Tests
{
    public class AssociationTests
    {
        [Test]
        public void Appointment_Should_Link_Patient_And_Doctor_BothWays()
        {
            var dept = TestHelper.CreateDepartment();
            var doctor = TestHelper.CreateDoctor(dept);
            var patient = TestHelper.CreatePatient(doctor);

            var apptTime = DateTime.Now.AddHours(2);
            var appt = new Appointment(patient, doctor, apptTime);

            Assert.That(patient.Appointments, Does.Contain(appt));
            Assert.That(doctor.ConductedAppointments.ContainsKey(apptTime), Is.True);
            Assert.That(appt.Patient, Is.EqualTo(patient));
            Assert.That(appt.Doctor, Is.EqualTo(doctor));
        }

        [Test]
        public void Department_Should_Assign_Employee_And_Set_BackReference()
        {
            var dept = TestHelper.CreateDepartment();
            var doctor = TestHelper.CreateDoctor(dept);

            Assert.That(dept.Employees, Does.Contain(doctor));
            Assert.That(doctor.Department, Is.EqualTo(dept));
            Assert.That(dept.TotalEmployees, Is.EqualTo(dept.Employees.Count));
        }
    }
}