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

            // Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                // Act
                configurationBuilder.AddYamlFile(path);
            });
            Assert.Equal("path", ex.ParamName);
            Assert.StartsWith("File path must be a non-empty string.", ex.Message);
        }

        [Fact]
        public void AddYamlFile_ThrowsIfFileDoesNotExistAtPath()
        {
            // Arrange
            var path = "file-does-not-exist.Yaml";
            var configurationBuilder = new ConfigurationBuilder();
            
            // Assert
            var ex = Assert.Throws<FileNotFoundException>(() =>
            {
                // Act
                configurationBuilder.AddYamlFile(path).Build();
            });
            Assert.StartsWith($"The configuration file '{path}' was not found and is not optional.", ex.Message);
        }
    }
}
