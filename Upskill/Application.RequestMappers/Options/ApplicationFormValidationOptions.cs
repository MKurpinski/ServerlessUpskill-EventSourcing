using System.Collections.Generic;
using System.Linq;

namespace Application.RequestMappers.Options
{
    public class ApplicationFormValidationOptions
    {
        public int MaxCvSizeInMegabytes { get; set; }
        public int MaxPhotoSizeInMegabytes { get; set; }
        public IReadOnlyCollection<string> AllowedPhotoFormats { get; set; }
        public IReadOnlyCollection<string> AllowedCvFormats { get; set; }
    }
}
