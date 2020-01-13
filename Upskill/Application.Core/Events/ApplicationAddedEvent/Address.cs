namespace Application.Core.Events.ApplicationAddedEvent
{
    public class Address
    {
        public string City { get; }
        public string Country { get; }

        public Address(string city, string country)
        {
            City = city;
            Country = country;
        }
    }
}
