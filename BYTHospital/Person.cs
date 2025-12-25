using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HospitalSystem
{
    public class Person
    {
     
        private string _name = "Unknown";
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        private string _surname = "Unknown";
        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surname cannot be empty.");
                _surname = value;
            }
        }

        private DateTime _dateOfBirth = DateTime.Today.AddYears(-18);
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Date of birth cannot be in the future.");
                _dateOfBirth = value;
            }
        }

        private string _nationalID = "00000000000";
        public string NationalID
        {
            get => _nationalID;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("National ID cannot be empty.");
                _nationalID = value;
            }
        }

        private string _gender = "Unknown";
        public string Gender
        {
            get => _gender;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Gender cannot be empty.");
                _gender = value;
            }
        }

        private string _phoneNumber = "000000000";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phone number cannot be empty.");
                _phoneNumber = value;
            }
        }

        private Address _address = new Address();
        public Address Address
        {
            get => _address;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(Address), "A person must have an address.");
                if (_address == value) return;

                _address.Person = null;
                _address = value;
                _address.Person = this;
            }
        }

       
        public bool IsPatient { get; private set; }
        public bool IsEmployee { get; private set; }

       
        [JsonIgnore] 
        public Department? Department { get; internal set; }

        private decimal _salary;
        public decimal Salary
        {
            get => _salary;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Salary), "Salary cannot be negative.");
                _salary = value;
            }
        }

        
        public EmployeeType EmployeeType { get; private set; } = EmployeeType.None;

       
        private string _registrationNumber = "00000";
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                if (!IsNurse) throw new InvalidOperationException("RegistrationNumber can be set only for nurses.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Registration number cannot be empty.");
                _registrationNumber = value;
            }
        }

        private string _shiftDetails = "N/A";
        public string ShiftDetails
        {
            get => _shiftDetails;
            set
            {
                if (!IsNurse) throw new InvalidOperationException("ShiftDetails can be set only for nurses.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Shift details cannot be empty.");
                _shiftDetails = value;
            }
        }

        
        private string _specialization = "General";
        public string Specialization
        {
            get => _specialization;
            set
            {
                if (!IsDoctor) throw new InvalidOperationException("Specialization can be set only for doctors.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Specialization cannot be empty.");
                _specialization = value;
            }
        }

        private string _licenseNumber = "00000";
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (!IsDoctor) throw new InvalidOperationException("LicenseNumber can be set only for doctors.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("License number cannot be empty.");
                _licenseNumber = value;
            }
        }

        
        public EmploymentType EmploymentType { get; private set; } = EmploymentType.None;

        public DateTime? EmploymentStartDate { get; private set; }
        public DateTime? EmploymentEndDate { get; private set; }

        public DateTime? ContractStartDate { get; private set; }
        public DateTime? ContractEndDate { get; private set; }

       
        public DoctorRole DoctorRoles { get; private set; } = DoctorRole.None;

        private string _consultingHours = "N/A";
        public string ConsultingHours
        {
            get => _consultingHours;
            set
            {
                if (!IsDoctor || !IsConsultant) throw new InvalidOperationException("ConsultingHours only for Consultant doctors.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Consulting hours cannot be empty.");
                _consultingHours = value;
            }
        }

        private string _surgeonSpeciality = "General Surgery";
        public string SurgeonSpeciality
        {
            get => _surgeonSpeciality;
            set
            {
                if (!IsDoctor || !IsSurgeon) throw new InvalidOperationException("SurgeonSpeciality only for Surgeon doctors.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surgeon speciality cannot be empty.");
                _surgeonSpeciality = value;
            }
        }

        
        public MedicalRecord? MedicalRecord { get; private set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [JsonIgnore]
        public Person? ResponsibleDoctor { get; private set; } 
        [JsonIgnore]
        public List<Appointment> Appointments { get; } = new();

        [JsonIgnore]
        public List<Surgery> Surgeries { get; } = new();

        [JsonIgnore]
        public List<RoomAssignment> RoomAssignments { get; } = new();

        
        [JsonIgnore]
        public Person? SupervisingDoctor { get; private set; } 

        [JsonIgnore]
        public List<Person> SupervisedDoctors { get; } = new(); 

        [JsonIgnore]
        public List<Person> ResponsibleForPatients { get; } = new(); 

       
        [JsonIgnore]
        public Dictionary<DateTime, Appointment> ConductedAppointments { get; } = new();

       
        [JsonIgnore] public bool IsDoctor => IsEmployee && EmployeeType == EmployeeType.Doctor;
        [JsonIgnore] public bool IsNurse => IsEmployee && EmployeeType == EmployeeType.Nurse;
        [JsonIgnore] public bool IsConsultant => IsDoctor && DoctorRoles.HasFlag(DoctorRole.Consultant);
        [JsonIgnore] public bool IsSurgeon => IsDoctor && DoctorRoles.HasFlag(DoctorRole.Surgeon);

        public Person()
        {
            _address.Person = this;
        }

      

        public void MakePatient(Person doctorResponsible)
        {
            if (doctorResponsible == null) throw new ArgumentNullException(nameof(doctorResponsible));
            if (!doctorResponsible.IsDoctor) throw new ArgumentException("Responsible person must be a Doctor.");

            IsPatient = true;
            MedicalRecord ??= new MedicalRecord(this);

            SetResponsibleDoctor(doctorResponsible);
        }

        public void MakeEmployee(EmployeeType type, Department dept, decimal salary)
        {
            if (type == EmployeeType.None) throw new ArgumentException("EmployeeType cannot be None.");
            if (dept == null) throw new ArgumentNullException(nameof(dept));

            IsEmployee = true;
            EmployeeType = type;
            Salary = salary;

            dept.AddEmployee(this);
        }

        public void SetDoctorEmployment(EmploymentType type, DateTime start, DateTime? end = null)
        {
            if (!IsDoctor) throw new InvalidOperationException("Only doctors can have employment type.");
            if (type == EmploymentType.None) throw new ArgumentException("EmploymentType cannot be None.");

            if (end.HasValue && end.Value < start)
                throw new ArgumentException("End date cannot be before start.");

            EmploymentType = type;

            if (type == EmploymentType.Permanent)
            {
                EmploymentStartDate = start;
                EmploymentEndDate = end;
                ContractStartDate = null;
                ContractEndDate = null;
            }
            else
            {
                ContractStartDate = start;
                ContractEndDate = end ?? start;
                EmploymentStartDate = null;
                EmploymentEndDate = null;
            }
        }

        public void SetDoctorRoles(DoctorRole roles)
        {
            if (!IsDoctor) throw new InvalidOperationException("Only doctors can have roles.");
            if (roles == DoctorRole.None)
                throw new ArgumentException("DoctorRoles must include at least one role (Consultant/Surgeon).");

            DoctorRoles = roles;
        }

        public void SetResponsibleDoctor(Person doctor)
        {
            if (!IsPatient) throw new InvalidOperationException("Only patients can have a responsible doctor.");
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (!doctor.IsDoctor) throw new ArgumentException("Responsible doctor must be a Doctor.");

            ResponsibleDoctor?.ResponsibleForPatients.Remove(this);
            ResponsibleDoctor = doctor;
            if (!doctor.ResponsibleForPatients.Contains(this))
                doctor.ResponsibleForPatients.Add(this);
        }

        public void SetSupervisor(Person? supervisorDoctor)
        {
            if (!IsDoctor) throw new InvalidOperationException("Only doctors can be supervised.");
            if (supervisorDoctor == this) throw new ArgumentException("A doctor cannot supervise themselves.");
            if (supervisorDoctor != null && !supervisorDoctor.IsDoctor)
                throw new ArgumentException("Supervisor must be a Doctor.");

            SupervisingDoctor?.SupervisedDoctors.Remove(this);
            SupervisingDoctor = supervisorDoctor;
            if (supervisorDoctor != null && !supervisorDoctor.SupervisedDoctors.Contains(this))
                supervisorDoctor.SupervisedDoctors.Add(this);
        }

       
        public bool HasActiveSurgery() => Surgeries.Any(s => s.EndTime == null);
        public bool HasActiveRoomAssignment() => RoomAssignments.Any(ra => ra.DischargeDate == null);

        internal void InternalAddAppointment(Appointment a)
        {
            if (!Appointments.Contains(a)) Appointments.Add(a);
        }

        internal void InternalRemoveAppointment(Appointment a) => Appointments.Remove(a);

        internal void InternalAddSurgery(Surgery s)
        {
            if (!Surgeries.Contains(s)) Surgeries.Add(s);
        }

        internal void InternalAddRoomAssignment(RoomAssignment ra)
        {
            if (!RoomAssignments.Contains(ra)) RoomAssignments.Add(ra);
        }

        internal void InternalAddConductedAppointment(Appointment a)
        {
            if (!IsDoctor) throw new InvalidOperationException("Only doctors can conduct appointments.");
            if (ConductedAppointments.ContainsKey(a.DateTime))
                throw new InvalidOperationException("This doctor already has an appointment at that time.");
            ConductedAppointments[a.DateTime] = a;
        }

        internal void InternalRemoveConductedAppointment(Appointment a)
        {
            if (ConductedAppointments.TryGetValue(a.DateTime, out var existing) && existing == a)
                ConductedAppointments.Remove(a.DateTime);
        }
    }
}
