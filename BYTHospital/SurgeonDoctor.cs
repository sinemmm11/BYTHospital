using System;

namespace HospitalSystem
{
    public class SurgeonDoctor : Doctor
    {
        private string _surgeonSpeciality = "General Surgery";
        public string SurgeonSpeciality
        {
            get => _surgeonSpeciality;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surgeon speciality cannot be empty.");
                _surgeonSpeciality = value;
            }
        }

        public SurgeonDoctor()
        {
            SurgeonSpeciality = "General Surgery";
        }
    }
}
