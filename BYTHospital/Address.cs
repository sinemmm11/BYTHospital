using System;

namespace HospitalSystem
{
    public class Address
    {
        private string _country;
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

        private string _city;
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

        private string _street;
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
    }
}