using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class SurgeryStaffParticipation
    {
        public static List<SurgeryStaffParticipation> Extent = new();

        public Surgery Surgery { get; private set; } = null!;
        public Employee StaffMember { get; private set; } = null!;

        private string _role = "Unknown";
        public string Role
        {
            get => _role;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Role cannot be empty.");
                _role = value;
            }
        }

        public SurgeryStaffParticipation()
        {
            Role = "Unknown";
            Extent.Add(this);
        }

        public SurgeryStaffParticipation(Surgery surgery, Employee staff, string role) : this()
        {
            Surgery = surgery ?? throw new ArgumentNullException(nameof(surgery));
            StaffMember = staff ?? throw new ArgumentNullException(nameof(staff));
            Role = role;

            
            surgery.AddStaffParticipation(this);
            staff.InternalAddSurgeryParticipation(this);
        }

        public static void SaveExtent(string file) =>
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file)) { Extent = new(); return; }
            Extent = JsonSerializer.Deserialize<List<SurgeryStaffParticipation>>(File.ReadAllText(file)) ?? new();
        }
    }
}
