using System;

namespace HospitalSystem
{
    public class ContractorDoctor : Doctor
    {
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
    }
}