using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public abstract class Employee : Person
    {
       
        public Department? Department { get; internal set; }

        private decimal _salary;
        public decimal Salary
        {
            get => _salary;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Salary), "Salary cannot be negative.");
                _salary = value;
            }
        }

       
        public List<SurgeryStaffParticipation> SurgeryParticipations { get; } = new();

        internal void InternalAddSurgeryParticipation(SurgeryStaffParticipation participation)
        {
            if (participation == null)
                throw new ArgumentNullException(nameof(participation));

            if (!SurgeryParticipations.Contains(participation))
                SurgeryParticipations.Add(participation);
        }

        internal void InternalRemoveSurgeryParticipation(SurgeryStaffParticipation participation)
        {
            if (participation == null)
                throw new ArgumentNullException(nameof(participation));

            SurgeryParticipations.Remove(participation);
        }
    }
}
