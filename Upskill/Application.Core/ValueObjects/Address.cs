namespace Application.Core.ValueObjects
{
    public class Address
    {
        public Address(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string City { get; }
        public string Country { get; }
    }
}
