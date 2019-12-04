namespace Application.Api.Options
{
    public class ApplicationFormOptions
    {
        public int MaxCvSizeInMegabytes { get; set; }
        public int MaxPhotoSizeInMegabytes { get; set; }
        public string[] AllowedPhotoFormats { get; set; }
        public string[] AllowedCvFormats { get; set; }
    }
}
