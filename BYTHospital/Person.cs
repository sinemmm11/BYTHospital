using System;

namespace HospitalSystem
{
    public abstract class Person
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

        private DateTime _dateOfBirth;
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

                if (_address != null) _address.Person = null;

                _address = value;
                _address.Person = this;
            }
        }

        protected Person()
        {
            _address.Person = this;
        }
    }
}
