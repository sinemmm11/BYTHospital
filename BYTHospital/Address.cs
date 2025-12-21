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

        public string State { get; set; } = "Unknown";
        public string Postcode { get; set; } = "Unknown";
        public string Building { get; set; } = string.Empty;

        
        public string? Apartment { get; set; } = null;

        public Person? Person { get; internal set; }

        public Address()
        {
            Country = "Unknown";
            City = "Unknown";
            Street = "Unknown";
        }
    }
}
