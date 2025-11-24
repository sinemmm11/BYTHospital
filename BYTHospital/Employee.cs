using System;

namespace HospitalSystem
{
    public abstract class Employee : Person
    {
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
    }
}
