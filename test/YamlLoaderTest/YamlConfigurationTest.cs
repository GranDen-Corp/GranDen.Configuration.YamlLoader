﻿using System;
using System.IO;
using GranDen.YamlLoader;
using GranDen.YamlLoaderTest.Util;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GranDen.YamlLoaderTest
{
    public class YamlConfigurationTest
    {
        private static YamlConfigurationProvider LoadProvider(string Yaml)
        {
            var p = new YamlConfigurationProvider(new YamlConfigurationSource { Optional = true });
            p.Load(TestStreamHelpers.StringToStream(Yaml));
            return p;
        }

        [Fact]
        public void LoadKeyValuePairsFromValidYaml()
        {
            var yaml = @"
                firstname: test
                test.last.name: last.name
                residential.address: 
                  street.name: Something street
                  zipcode: '12345'
                ";
            var yamlConfigSrc = LoadProvider(yaml);

            Assert.Equal("test", yamlConfigSrc.Get("firstname"));
            Assert.Equal("last.name", yamlConfigSrc.Get("test.last.name"));
            Assert.Equal("Something street", yamlConfigSrc.Get("residential.address:STREET.name"));
            Assert.Equal("12345", yamlConfigSrc.Get("residential.address:zipcode"));
        }

        [Fact]
        public void LoadMethodCanHandleBlankValue()
        {
            var yaml = @"
            name: 
        ";
            var yamlConfigSrc = LoadProvider(yaml);
            Assert.Equal(string.Empty, yamlConfigSrc.Get("name"));
        }

        [Fact]
        public void LoadMethodCanHandleEmptyValue()
        {
            var yaml = @"
            name: ''
        ";
            var yamlConfigSrc = LoadProvider(yaml);
            Assert.Equal(string.Empty, yamlConfigSrc.Get("name"));
        }

        [Fact]
        public void SupportAndIgnoreComments()
        {
            var yaml = @"# Comments 
                        # Comments
                        name: test
                        # Comments
                        address: 
                          street: Something street # Comments
                          zipcode: '12345'
                    ";
            var yamlConfigSrc = LoadProvider(yaml);
            Assert.Equal("test", yamlConfigSrc.Get("name"));
            Assert.Equal("Something street", yamlConfigSrc.Get("address:street"));
            Assert.Equal("12345", yamlConfigSrc.Get("address:zipcode"));
        }


        [Fact]
        public void ReturnEmptyConfigWhenFileIsEmpty()
        {
            var yaml = @"";

            var yamlConfigSrc = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml) };

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSrc);
            var config = configurationBuilder.Build();

            Assert.Empty(config.AsEnumerable());
        }

        [Fact]
        public void ThrowExceptionWhenUnexpectedFirstCharacterInScalarValue()
        {
            var yaml = @"
                        name: test
                        address: 
                          # Can't start scalar values with unclosed {
                          street: {Something street
                          zipcode: '12345'
                         ";
            var exception = Assert.Throws<FormatException>(() => LoadProvider(yaml));
            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenUnexpectedFirstCharacterInKeyValue()
        {
            var yaml = @"
                        name: test
                        address: 
                          street: Something street
                          zipcode: '12345'
                        # Can't start left value with {
                        {phone: mobile 
                         ";
            var exception = Assert.Throws<FormatException>(() => LoadProvider(yaml));
            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenUnexpectedEndOfFile()
        {
            var yaml = @"
                        name: test
                        address 
                         ";
            var exception = Assert.Throws<FormatException>(() => LoadProvider(yaml));
            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenPassingNullAsFilePath()
        {
            var expectedMsg = new ArgumentException("File path must be a non-empty string.", "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => new ConfigurationBuilder().AddYamlFile(path: null));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void ThrowExceptionWhenPassingEmptyStringAsFilePath()
        {
            var expectedMsg = new ArgumentException("File path must be a non-empty string.", "path").Message;

            var exception = Assert.Throws<ArgumentException>(() => new ConfigurationBuilder().AddYamlFile(string.Empty));

            Assert.Equal(expectedMsg, exception.Message);
        }

        [Fact]
        public void YamlConfiguration_Throws_On_Missing_Configuration_File()
        {
            var config = new ConfigurationBuilder().AddYamlFile("NotExistingConfig.Yaml", optional: false);
            var exception = Assert.Throws<FileNotFoundException>(() => config.Build());

            // Assert
            Assert.StartsWith($"The configuration file 'NotExistingConfig.Yaml' was not found and is not optional.", exception.Message);
        }

        [Fact]
        public void YamlConfiguration_Does_Not_Throw_On_Optional_Configuration()
        {
            var config = new ConfigurationBuilder().AddYamlFile("NotExistingConfig.Yaml", optional: true).Build();
        }
    }
}