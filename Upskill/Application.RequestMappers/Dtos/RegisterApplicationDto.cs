using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.Dtos
{
    public class RegisterApplicationDto
    {
        public IFormFile Cv { get; }
        public IFormFile Photo { get; }
        public CandidateDto Candidate { get; }

        public RegisterApplicationDto(IFormFile cv, IFormFile photo, CandidateDto candidate)
        {
            Cv = cv;
            Photo = photo;
            Candidate = candidate;
        }
    }
}
