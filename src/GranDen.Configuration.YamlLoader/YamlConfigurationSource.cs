using Microsoft.Extensions.Configuration;

namespace GranDen.Configuration.YamlLoader
{
    /// <summary>
    /// Yaml configuration source
    /// </summary>
    public class YamlFileConfigurationSource : FileConfigurationSource
    {
        /// <inheritdoc/>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new YamlConfigurationProvider(this);
        }
    }
}
