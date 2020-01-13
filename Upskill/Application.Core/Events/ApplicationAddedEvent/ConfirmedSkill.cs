using System;

namespace Application.Core.Events.ApplicationAddedEvent
{
    public class ConfirmedSkill
    {
        public string Name { get; }
        public DateTime DateOfAchievement { get; }

        public ConfirmedSkill(string name, DateTime dateOfAchievement)
        {
            Name = name;
            DateOfAchievement = dateOfAchievement;
        }
    }
}
