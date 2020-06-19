using System;
using System.IO;
using GranDen.Configuration.YamlLoader;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YamlLoaderTest
{
    public class YamlConfigurationExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddYamlFile_ThrowsIfFilePathIsNullOrEmpty(string path)
        {
            // Arrange
            var configurationBuilder = new ConfigurationBuilder();

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => YamlConfigurationExtensions.AddYamlFile(configurationBuilder, path));
            Assert.Equal("path", ex.ParamName);
            Assert.StartsWith("File path must be a non-empty string.", ex.Message);
        }

        [Fact]
        public void AddYamlFile_ThrowsIfFileDoesNotExistAtPath()
        {
            // Arrange
            var path = "file-does-not-exist.Yaml";

            // Act and Assert
            var ex = Assert.Throws<FileNotFoundException>(() => new ConfigurationBuilder().AddYamlFile(path).Build());
            Assert.StartsWith($"The configuration file '{path}' was not found and is not optional.", ex.Message);
        }
    }
}