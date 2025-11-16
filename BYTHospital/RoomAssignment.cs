using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HospitalSystem
{
    public class RoomAssignment
    {
        public static List<RoomAssignment> Extent = new List<RoomAssignment>();

        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }

        public RoomAssignment()
        {
            Extent.Add(this);
        }

        public static void SaveExtent(string file)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(Extent));
        }

        public static void LoadExtent(string file)
        {
            if (File.Exists(file))
                Extent = JsonSerializer.Deserialize<List<RoomAssignment>>(File.ReadAllText(file));
        }
    }
}