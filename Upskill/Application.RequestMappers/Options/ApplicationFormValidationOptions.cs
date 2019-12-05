namespace Application.RequestMappers.Options
{
    public class ApplicationFormValidationOptions
    {
        public int MaxCvSizeInMegabytes { get; set; }
        public int MaxPhotoSizeInMegabytes { get; set; }
        public string[] AllowedPhotoFormats { get; set; }
        public string[] AllowedCvFormats { get; set; }
    }
}
