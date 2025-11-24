using System;

namespace HospitalSystem
{
    public class Address
    {
        private string _country = "Unknown";
        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Country cannot be empty.");
                _country = value;
            }
        }

        private string _city = "Unknown";
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City cannot be empty.");
                _city = value;
            }
        }

        private string _street = "Unknown";
        public string Street
        {
            get => _street;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Street cannot be empty.");
                _street = value;
            }
        }

        public string State { get; set; }
        public string Postcode { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }

        public Address()
        {
            Country = "Unknown";
            City = "Unknown";
            Street = "Unknown";
            State = "Unknown";
            Postcode = "Unknown";
            Building = string.Empty;
            Apartment = string.Empty;
        }
    }
}