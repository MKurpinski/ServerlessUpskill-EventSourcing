using System;

namespace Application.Commands.Commands
{
    public class RegisterApplicationCommand : ICommand
    {
        public ApplicationFile Cv { get; }
        public ApplicationFile Photo { get; }
        public Candidate Candidate { get; }
        public DateTime CreationTime { get; }

        public RegisterApplicationCommand(
            ApplicationFile cv,
            ApplicationFile photo,
            Candidate candidate,
            DateTime creationTime)
        {
            Cv = cv;
            Photo = photo;
            Candidate = candidate;
            CreationTime = creationTime;
        }
    }
}
