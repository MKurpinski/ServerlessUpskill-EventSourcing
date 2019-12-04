using System;

namespace Application.Api.Commands.RegisterApplicationCommand
{
    public class RegisterApplicationCommand : ICommand
    {
        public byte[] Cv { get; }
        public byte[] Photo { get; }
        public Candidate Candidate { get; }
        public DateTime CreationTime { get; }

        public RegisterApplicationCommand(
            byte[] cv,
            byte[] photo,
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
