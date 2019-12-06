using System;

namespace Application.DataStorage.Model
{
    public class Application
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; }
        public string PhotoUri { get; }
        public string CvUri { get; }
        public string Category { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Application(string id, DateTime creationTime, string photoUri, string cvUri, string category, string firstName, string lastName)
        {
            Id = id;
            CreationTime = creationTime;
            PhotoUri = photoUri;
            CvUri = cvUri;
            Category = category;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
