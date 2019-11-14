using System;

namespace OAuthService.Core.ModelBinders
{
    public class ClientCultureInfo
    {
        public ClientCultureInfo()
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Indochina Time");
        }

        public ClientCultureInfo(TimeZoneInfo timeZoneInfo)
        {
            TimeZone = timeZoneInfo;
        }

        public TimeZoneInfo TimeZone { get; set; }

        public DateTime GetClientLocalTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZone);
        }

        public DateTime GetUtcTime(DateTime datetime)
        {
            return TimeZoneInfo.ConvertTime(datetime, TimeZone)
                .ToUniversalTime();
        }
    }
}