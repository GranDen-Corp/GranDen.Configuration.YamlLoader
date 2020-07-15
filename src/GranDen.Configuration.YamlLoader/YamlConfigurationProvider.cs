using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace GranDen.Configuration.YamlLoader
{
    /// <summary>
    /// Configuration Provider for YAML support.
    /// </summary>
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="source"></param>
        public YamlConfigurationProvider(YamlFileConfigurationSource source) : base(source) { }

        /// <inheritdoc/>
        public override void Load(Stream stream)
        {
            var parser = new YamlConfigurationFileParser();
            try
            {
                Data = parser.Parse(stream);
            }
            catch (YamlException ex)
            {
                throw new FormatException("YAML file parse error", ex);
            }
        }
    }
}
