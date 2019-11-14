using System;
using System.IO;

namespace OAuthService.Core.Helpers
{
    public class EnvironmentHelper
    {
        public static string Environment
        {
            get
            {
                var path = $"{AppContext.BaseDirectory}\\Environment.txt";
                if (!File.Exists(path)) return "Development";
                return File.ReadAllText(path);
            }
        }

        public const string DEVELOPMENT = "Development";
    }
}
