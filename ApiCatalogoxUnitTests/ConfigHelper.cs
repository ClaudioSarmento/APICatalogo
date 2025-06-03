using Microsoft.Extensions.Configuration;

namespace ApiCatalagoxUnitTests
{
    public class ConfigHelper
    {
        public static IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Local.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
