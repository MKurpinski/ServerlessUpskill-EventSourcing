using System;

namespace Application.Commands.Commands
{
    public class SaveApplicationCommand
    {
        public DateTime CreationTime { get; }
        public string PhotoId { get; }
        public string CvId { get; }
        public string Category { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public SaveApplicationCommand(
            string firstName,
            string lastName,
            string photoId,
            string cvId,
            string category,
            DateTime creationTime)
        {
            CreationTime = creationTime;
            PhotoId = photoId;
            CvId = cvId;
            Category = category;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
