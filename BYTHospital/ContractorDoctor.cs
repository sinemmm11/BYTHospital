using System;

namespace HospitalSystem
{
    public class ContractorDoctor : Doctor
    {
        public DateTime ContractStartDate { get; private set; }
        public DateTime ContractEndDate { get; private set; }

        public void SetContractPeriod(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("Contract end date cannot be before the start date.");
            ContractStartDate = startDate;
            ContractEndDate = endDate;
        }
    }
}
