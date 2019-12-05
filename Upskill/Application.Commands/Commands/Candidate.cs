namespace Application.Commands.Commands
{
    public class Candidate
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Category { get; }

        public Candidate(
            string firstName,
            string lastName,
            string category)
        {
            FirstName = firstName;
            LastName = lastName;
            Category = category;
        }
    }
}
