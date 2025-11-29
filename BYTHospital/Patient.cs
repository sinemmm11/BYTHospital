using System;
using System.Collections.Generic;
using System.IO;

namespace HospitalSystem
{
    public class Patient : Person
    {
        public static List<Patient> Extent = new List<Patient>();

        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Age), "Age cannot be negative.");
                _age = value;
            }
        }

        private DateTime _birthDate = DateTime.Today.AddYears(-18);
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value > DateTime.Today)
                    throw new ArgumentException("Birth date cannot be in the future");
                _birthDate = value;
            }
        }

        public int CalculatedAge
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public string? MiddleName { get; private set; }

        public void SetMiddleName(string? middleName)
        {
            if (middleName != null && string.IsNullOrWhiteSpace(middleName))
                throw new ArgumentException("Middle name cannot be only whitespace");
            MiddleName = middleName;
        }

        public List<string> Allergies { get; } = new List<string>();

        public void AddAllergy(string allergy)
        {
            if (string.IsNullOrWhiteSpace(allergy))
                throw new ArgumentException("Allergy cannot be empty");

            if (!Allergies.Contains(allergy))
                Allergies.Add(allergy);
        }

        public Patient()
        {
            Extent.Add(this);
        }

        
        public static void SaveExtent(string file)
        {
           
            File.WriteAllText(file, Extent.Count.ToString());
        }

        public static void LoadExtent(string file)
        {
           
            Extent = new List<Patient>();

            if (!File.Exists(file))
                return;

            var text = File.ReadAllText(file);
            if (int.TryParse(text, out int count) && count >= 0)
            {
               
                for (int i = 0; i < count; i++)
                {
                    new Patient();
                }
            }
        }
    }
}
