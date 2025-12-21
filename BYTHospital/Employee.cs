using System;
using System.Collections.Generic;

namespace HospitalSystem
{
    public abstract class Employee : Person
    {
        private Department? _department;
        public Department? Department
        {
            get => _department;
            set
            {
                if (_department == value) return;

                if (value != null && _department != null)
                {
                    throw new InvalidOperationException("This employee is already assigned to a department. Remove them first.");
                }

                _department?.RemoveEmployee(this);
                _department = value;
                _department?.AddEmployee(this);
            }
        }

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
