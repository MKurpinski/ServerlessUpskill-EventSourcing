using System;
using System.Collections.Generic;
using System.Linq;
using Application.PushNotifications.Enums;
using Microsoft.Azure.NotificationHubs;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.PushNotifications.Factories
{
    public class RegistrationDescriptionFactory : IRegistrationDescriptionFactory
    {
        public IDataResult<RegistrationDescription> Build(string registrationId, string platform, string handle, string[] tags)
        {
            if (handle == null)
            {
                return new FailedDataResult<RegistrationDescription>(nameof(handle), "Handle cannot be null");
            }

            var isPlatformSupported = Enum.TryParse<MobilePlatform>(platform, true, out var platformEnum);
            if (!isPlatformSupported)
            {
                return new FailedDataResult<RegistrationDescription>(nameof(platform), "Platform is not supported");
            }

            var platformSpecific = this.BuildPlatformSpecific(platformEnum, handle);

            if (tags?.Any() == true)
            {
                platformSpecific.Tags = new HashSet<string>(tags);
            }

            platformSpecific.RegistrationId = registrationId;

            return new SuccessfulDataResult<RegistrationDescription>(platformSpecific);
        }

        private RegistrationDescription BuildPlatformSpecific(MobilePlatform platform, string handle)
        {
            switch (platform)
            {
                case MobilePlatform.Apns:
                    return new AppleRegistrationDescription(handle);
                case MobilePlatform.Fcm:
                    return new FcmRegistrationDescription(handle);
                default:
                    return null;
            }
        }
    }
}