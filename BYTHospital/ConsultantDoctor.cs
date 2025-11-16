using System;

namespace HospitalSystem
{
    public class ConsultantDoctor : Doctor
    {
        private string _consultingHours;
        public string ConsultingHours
        {
            get => _consultingHours;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Consulting hours cannot be empty.");
                _consultingHours = value;
            }
        }
    }
}