using System.Collections.Generic;
using System.Linq;

namespace Application.RequestMappers.Options
{
    public class ApplicationFormValidationOptions
    {
        public int MaxCvSizeInMegabytes { get; set; }
        public int MaxPhotoSizeInMegabytes { get; set; }
        public IList<string> PhotoFormats => this.SplitOptions(this.AllowedPhotoFormats);
        public IList<string> CvFormats => this.SplitOptions(this.AllowedCvFormats);

        public string AllowedPhotoFormats { get; set; }
        public string AllowedCvFormats { get; set; }

        private IList<string> SplitOptions(string options)
        {
            const char splitter = ',';
            return options?.Split(splitter).ToList() ?? new List<string>();
        }
}
}
