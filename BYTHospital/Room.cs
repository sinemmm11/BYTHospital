using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class Room
    {
        public static List<Room> Extent = new List<Room>();

        private string _roomNumber;
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

        private string _type;
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

        public bool IsAvailable { get; set; }

        // Multi-valued: RoomAssignments
        public List<RoomAssignment> Assignments { get; } = new List<RoomAssignment>();

        // Derived: IsFull (testte kullanılıyor)
        public bool IsFull => Assignments.Count >= Capacity;

        public Room()
        {
            Capacity = 1;
            Extent.Add(this);
        }

        public void AddAssignment(RoomAssignment assignment)
        {
            if (assignment == null) throw new ArgumentNullException(nameof(assignment));
            if (IsFull)
                throw new InvalidOperationException("Room is already full");

            if (!Assignments.Contains(assignment))
                Assignments.Add(assignment);
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<Room>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(file));
            Extent = data ?? new List<Room>();
        }
    }
}