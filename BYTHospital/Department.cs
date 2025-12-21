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

        private int _totalEmployees = 0;
        public int TotalEmployees => _totalEmployees;

        public PermanentDoctor? Head { get; private set; }

        public void SetHead(PermanentDoctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (!Employees.Contains(doctor))
            {
                throw new ArgumentException("The department head must be an employee of the department.");
            }
            
            if (doctor.HeadedDepartment != null && doctor.HeadedDepartment != this)
            {
                throw new InvalidOperationException("This doctor is already the head of another department.");
            }

            if (Head == doctor) return;
            if (Head != null) Head.HeadedDepartment = null;

            Head = doctor;

            Head.HeadedDepartment = this;
        }

        public List<Employee> Employees { get; } = new();
        private Dictionary<string, Employee> _employeesById = new();

        public Employee? GetEmployeeById(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId)) return null;
            _employeesById.TryGetValue(employeeId, out var emp);
            return emp;
        }

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
            
            if (emp.Department != null && emp.Department != this)
            {
                throw new InvalidOperationException("This employee is already assigned to another department.");
            }

            if (!Employees.Contains(emp))
            {
                Employees.Add(emp);
                _employeesById[emp.NationalID] = emp; // Update Qualifier
                _totalEmployees++; // Update stored attribute
                emp.Department = this; // Reverse connection
            }
        }

        public void RemoveEmployee(Employee emp)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));
            if (Employees.Contains(emp))
            {
                Employees.Remove(emp);
                _employeesById.Remove(emp.NationalID); // Update Qualifier
                _totalEmployees--; // Update stored attribute
                emp.Department = null; // Break reverse connection
                
                if (emp is Doctor doc) Doctors.Remove(doc);
                if (emp is Nurse nurse) Nurses.Remove(nurse);
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

        public void RemoveRoom(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));
            if (Rooms.Contains(room))
            {
                Rooms.Remove(room);
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
