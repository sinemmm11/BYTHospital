using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class SurgeryStaffParticipation
    {
        public static List<SurgeryStaffParticipation> Extent = new List<SurgeryStaffParticipation>();

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

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (!File.Exists(file))
            {
                Extent = new List<SurgeryStaffParticipation>();
                return;
            }

            var data = JsonSerializer.Deserialize<List<SurgeryStaffParticipation>>(File.ReadAllText(file));
            Extent = data ?? new List<SurgeryStaffParticipation>();
        }
    }
}