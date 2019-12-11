using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Commands
{
    public class RecompensateApplicationProcessCommand
    {
        public string Id { get; }
        public ApplicationFile Photo { get; }
        public ApplicationFile Cv { get; }

        public RecompensateApplicationProcessCommand(
            string id,
            ApplicationFile photo,
            ApplicationFile cv)
        {
            Id = id;
            Photo = photo;
            Cv = cv;
        }
    }
}
