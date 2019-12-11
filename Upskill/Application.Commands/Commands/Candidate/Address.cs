namespace Application.Commands.Commands.Candidate
{
    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }

        public Address(string city, string country)
        {
            City = city;
            Country = country;
        }
    }
}
