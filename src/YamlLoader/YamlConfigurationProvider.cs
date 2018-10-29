using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace GranDen.YamlLoader
{
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(FileConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new YamlConfigurationFileParser();
            try
            {
                var settingValue = parser.Parse(stream);
                //TODO: send data to exporter
                Data = settingValue;
            }
            catch (YamlException ex)
            {
                throw new FormatException("YAML file parse error", ex);
            }
        }
    }
}