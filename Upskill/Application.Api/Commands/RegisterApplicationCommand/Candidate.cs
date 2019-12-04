namespace Application.Api.Commands.RegisterApplicationCommand
{
    public class Candidate
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Candidate(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
