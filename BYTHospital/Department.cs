using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Department
    {
        public static List<Department> Extent = new();

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

       
        public int TotalEmployees => Employees.Count;

        public List<Employee> Employees { get; } = new();
        public List<Doctor> Doctors { get; } = new();
        public List<Nurse> Nurses { get; } = new();
        public List<Room> Rooms { get; } = new();

        public Department()
        {
            Id = Guid.NewGuid();
            Name = "Unknown";
            Location = "Unknown";
            Extent.Add(this);
        }

        public Department(string name) : this() => Name = name;

        public void AddEmployee(Employee emp)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));
            if (!Employees.Contains(emp))
            {
                Employees.Add(emp);
                emp.Department = this;
            }
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            AddEmployee(doctor);
            if (!Doctors.Contains(doctor)) Doctors.Add(doctor);
        }

        public void AddNurse(Nurse nurse)
        {
            if (nurse == null) throw new ArgumentNullException(nameof(nurse));
            AddEmployee(nurse);
            if (!Nurses.Contains(nurse)) Nurses.Add(nurse);
        }

        public void AddRoom(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));
            if (!Rooms.Contains(room))
            {
                Rooms.Add(room);
                room.Department = this;
            }
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Department>>(File.ReadAllText(file)) ?? new();
        }
    }
}
