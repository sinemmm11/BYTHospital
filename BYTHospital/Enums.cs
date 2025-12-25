namespace HospitalSystem
{
    public enum EmployeeType
    {
        None = 0,
        Doctor = 1,
        Nurse = 2
    }

    public enum EmploymentType
    {
        None = 0,
        Permanent = 1,
        Contractor = 2
    }

    [System.Flags]
    public enum DoctorRole
    {
        None = 0,
        Consultant = 1,
        Surgeon = 2
    }
}