using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class SurgeryStaffParticipation
    {
        public static List<SurgeryStaffParticipation> Extent = new();

        private Surgery _surgery = default!;
        public Surgery Surgery
        {
            get => _surgery;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Surgery));
                if (_surgery == value) return;

                _surgery?.Staff.Remove(this);
                _surgery = value;
                _surgery.AddStaffParticipation(this);
            }
        }

        private Person _staffMember = default!;
        public Person StaffMember
        {
            get => _staffMember;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(StaffMember));
                if (!value.IsEmployee) throw new ArgumentException("StaffMember must be IsEmployee=true.");
                _staffMember = value;
            }
        }

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

        public SurgeryStaffParticipation(Surgery surgery, Person staff, string role)
        {
            Surgery = surgery ?? throw new ArgumentNullException(nameof(surgery));
            StaffMember = staff ?? throw new ArgumentNullException(nameof(staff));
            Role = role;

            Extent.Add(this);
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