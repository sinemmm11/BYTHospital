using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        private int _totalEmployees = 0;
        public int TotalEmployees => _totalEmployees;

        [JsonIgnore]
        public Person? Head { get; private set; } 
        public void SetHead(Person doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (!doctor.IsDoctor) throw new ArgumentException("Head must be a Doctor.");
            if (doctor.EmploymentType != EmploymentType.Permanent)
                throw new ArgumentException("Head must be a Permanent doctor.");

            if (!Employees.Contains(doctor))
                throw new ArgumentException("The department head must be an employee of the department.");

            Head = doctor;
        }

        public List<Person> Employees { get; } = new();
        private readonly Dictionary<string, Person> _employeesById = new();

        public Person? GetEmployeeById(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return null;
            _employeesById.TryGetValue(employeeId, out var emp);
            return emp;
        }

        public List<Room> Rooms { get; } = new();

        public Department()
        {
            Id = Guid.NewGuid();
            Extent.Add(this);
        }

        public Department(string name) : this() => Name = name;

        public void AddEmployee(Person emp)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));
            if (!emp.IsEmployee)
                throw new ArgumentException("Only IsEmployee=true persons can be added as employees.");

            if (emp.Department != null && emp.Department != this)
                throw new InvalidOperationException("This employee is already assigned to another department.");

            if (!Employees.Contains(emp))
            {
                Employees.Add(emp);
                _employeesById[emp.NationalID] = emp;
                _totalEmployees++;
                emp.Department = this;
            }
        }

        public void RemoveEmployee(Person emp)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));
            if (Employees.Remove(emp))
            {
                _employeesById.Remove(emp.NationalID);
                _totalEmployees--;
                emp.Department = null;
            }
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

        public void RemoveRoom(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));
            if (Rooms.Remove(room))
            {
                room.Department = null;
                Room.Extent.Remove(room);
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
