using System;

namespace HospitalSystem
{
    public class PermanentDoctor : Doctor
    {
        public DateTime EmploymentStartDate { get; private set; }
        public DateTime? EmploymentEndDate { get; private set; }

        public void SetEmploymentPeriod(DateTime startDate, DateTime? endDate)
        {
            if (endDate.HasValue && endDate.Value < startDate)
            {
                throw new ArgumentException("Employment end date cannot be before the start date.");
            }
            EmploymentStartDate = startDate;
            EmploymentEndDate = endDate;
        }
    }
}