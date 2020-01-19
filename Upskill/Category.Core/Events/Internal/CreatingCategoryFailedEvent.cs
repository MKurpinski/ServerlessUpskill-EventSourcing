﻿using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class CreatingCategoryFailedEvent : BaseStatusEvent
    {
        public CreatingCategoryFailedEvent(
            CategoryModificationStatus status,
            string correlationId) 
            : base(status.ToString(), correlationId)
        {
        }
    }
}
