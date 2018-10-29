using Microsoft.Extensions.Configuration;

namespace GranDen.YamlLoader
{
    public class FileConfigurationSource : Microsoft.Extensions.Configuration.FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new YamlConfigurationProvider(this);
        }
    }
}