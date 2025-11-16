using System;

namespace HospitalSystem
{
    public class PermanentDoctor : Doctor
    {
        public DateTime EmploymentStartDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
    }
}