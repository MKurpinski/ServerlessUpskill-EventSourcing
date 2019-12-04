using Microsoft.AspNetCore.Http;

namespace Application.RequestDeserializers.Dtos
{
    public class AddApplicationFormDto
    {
        public byte[] Cv { get; }
        public byte[] Photo { get; }
        public CandidateDto Candidate { get; }

        public AddApplicationFormDto(byte[] cv, byte[] photo, CandidateDto candidate)
        {
            Cv = cv;
            Photo = photo;
            Candidate = candidate;
        }
    }
}
