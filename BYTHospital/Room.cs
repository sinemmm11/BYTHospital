using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Room
    {
        public static List<Room> Extent = new();

        private Department? _department;
        public Department? Department
        {
            get => _department;
            set
            {
                if (_department == value) return;

                _department?.RemoveRoom(this);
                _department = value;
                _department?.AddRoom(this);
            }
        }

        private string _roomNumber = "000";
        public string RoomNumber
        {
            get => _roomNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Room number cannot be empty.");
                _roomNumber = value;
            }
        }

        private string _type = "Standard";
        public string Type
        {
            get => _type;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Room type cannot be empty.");
                _type = value;
            }
        }

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Capacity), "Capacity must be > 0.");
                _capacity = value;
            }
        }

        private bool _isOutOfService = false;
        public bool IsAvailable => !IsFull && !_isOutOfService;

        public void SetOutOfService(bool status)
        {
            _isOutOfService = status;
        }

        public List<RoomAssignment> Assignments { get; } = new();

        
        public bool IsFull => Assignments.Count >= Capacity;

        public Room()
        {
            Capacity = 1;
            RoomNumber = "000";
            Type = "Standard";
            Extent.Add(this);
        }

        public void AddAssignment(RoomAssignment assignment)
        {
            if (assignment == null) throw new ArgumentNullException(nameof(assignment));
            if (IsFull) throw new InvalidOperationException("Room is already full");
            if (!Assignments.Contains(assignment)) Assignments.Add(assignment);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(file)) ?? new();
        }
    }
}
