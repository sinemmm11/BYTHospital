using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Department
    {
        public static List<Department> Extent = new List<Department>();

        public Guid Id { get; private set; }

        private string _name = "Unknown";
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Department name cannot be empty.");
                _name = value;
            }
        }

        private string _location = "Unknown";
        public string Location
        {
            get => _location;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Department location cannot be empty.");
                _location = value;
            }
        }

        public int TotalEmployees { get; set; }

        // Multi-valued ilişkiler (testte kullanılıyor)
        public List<Doctor> Doctors { get; } = new List<Doctor>();
        public List<Nurse> Nurses { get; } = new List<Nurse>();

        public Department()
        {
            Id = Guid.NewGuid();
            Name = "Unknown";
            Location = "Unknown";
            Extent.Add(this);
        }

        public Department(string name) : this()
        {
            Name = name;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (!Doctors.Contains(doctor))
            {
                Doctors.Add(doctor);
                doctor.Department = this;
            }
        }

        public void AddNurse(Nurse nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            if (!Nurses.Contains(nurse))
            {
                Nurses.Add(nurse);
                nurse.Department = this;
            }
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<Department>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Department>>(File.ReadAllText(file));
            Extent = data ?? new List<Department>();
        }
    }
}