﻿using System;

namespace Application.Core.Events.ApplicationChangedEvent
{
    public class ConfirmedSkill
    {
        public string Name { get; set; }
        public DateTime DateOfAchievement { get; set; }

        public ConfirmedSkill(string name, DateTime dateOfAchievement)
        {
            Name = name;
            DateOfAchievement = dateOfAchievement;
        }
    }
}
