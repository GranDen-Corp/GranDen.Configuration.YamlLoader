﻿using System.Linq;
using GranDen.Configuration.YamlLoader;
using InRamStreamHelper;
using YamlLoaderTest.Util;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YamlLoaderTest
{
    public class SequenceTests
    {
        [Fact]
        public void SequencesAreConvertedToKeyValuePairs()
        {
            var yaml = @"
                ip:
                - '1.2.3.4'
                - '7.8.9.10'
                - '11.12.13.14'
            ";

            var yamlConfigSource = new YamlConfigurationProvider(new YamlFileConfigurationSource());
            yamlConfigSource.Load(InRamStreamUtil.StringToStream(yaml));

            Assert.Equal("1.2.3.4", yamlConfigSource.Get("ip:0"));
            Assert.Equal("7.8.9.10", yamlConfigSource.Get("ip:1"));
            Assert.Equal("11.12.13.14", yamlConfigSource.Get("ip:2"));
        }

        [Fact]
        public void SequenceOfObjects()
        {
            var yaml = @"
                ip:
                - address: '1.2.3.4'
                  hidden: false
                - address: '5.6.7.8'
                  hidden: true
            ";

            var yamlConfigSource = new YamlConfigurationProvider(new YamlFileConfigurationSource());
            yamlConfigSource.Load(InRamStreamUtil.StringToStream(yaml));

            Assert.Equal("1.2.3.4", yamlConfigSource.Get("ip:0:address"));
            Assert.Equal("false", yamlConfigSource.Get("ip:0:hidden"));
            Assert.Equal("5.6.7.8", yamlConfigSource.Get("ip:1:address"));
            Assert.Equal("true", yamlConfigSource.Get("ip:1:hidden"));
        }

        [Fact]
        public void NestedSequences()
        {
            var yaml = @"
                'ip': 
                - 
                  - '1.2.3.4'
                  - '5.6.7.8'
                - 
                  - '9.10.11.12'
                  - '13.14.15.16'
                   ";

            var yamlConfigSource = new YamlConfigurationProvider(new YamlFileConfigurationSource());
            yamlConfigSource.Load(InRamStreamUtil.StringToStream(yaml));

            Assert.Equal("1.2.3.4", yamlConfigSource.Get("ip:0:0"));
            Assert.Equal("5.6.7.8", yamlConfigSource.Get("ip:0:1"));
            Assert.Equal("9.10.11.12", yamlConfigSource.Get("ip:1:0"));
            Assert.Equal("13.14.15.16", yamlConfigSource.Get("ip:1:1"));
        }

        [Fact]
        public void ImplicitSequenceItemReplacement()
        {
            var yaml1 = @"
                'ip': 
                - 1.2.3.4
                - '7.8.9.10'
                - '11.12.13.14'
                ";

            var yaml2 = @"
                'ip': 
                - '15.16.17.18'
            ";

            var yamlConfigSource1 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml1)};
            var yamlConfigSource2 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml2)};

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSource1);
            configurationBuilder.Add(yamlConfigSource2);
            var config = configurationBuilder.Build();

            Assert.Equal(3, config.GetSection("ip").GetChildren().Count());
            Assert.Equal("15.16.17.18", config["ip:0"]);
            Assert.Equal("7.8.9.10", config["ip:1"]);
            Assert.Equal("11.12.13.14", config["ip:2"]);
        }

        [Fact]
        public void ExplicitSequenceReplacement()
        {
            var yaml1 = @"
                'ip': 
                - 1.2.3.4
                - '7.8.9.10'
                - '11.12.13.14'
                ";

            var yaml2 = @"
                'ip': 
                  1: '15.16.17.18'
            ";

            var yamlConfigSource1 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml1)};
            var yamlConfigSource2 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml2)};

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSource1);
            configurationBuilder.Add(yamlConfigSource2);
            var config = configurationBuilder.Build();

            Assert.Equal(3, config.GetSection("ip").GetChildren().Count());
            Assert.Equal("1.2.3.4", config["ip:0"]);
            Assert.Equal("15.16.17.18", config["ip:1"]);
            Assert.Equal("11.12.13.14", config["ip:2"]);
        }

        [Fact]
        public void SequenceMerge()
        {
            var yaml1 = @"
               'ip': 
                - 1.2.3.4
                - '7.8.9.10'
                - '11.12.13.14'
                ";

            var yaml2 = @"
                'ip': 
                  3: '15.16.17.18'
            ";

            var yamlConfigSource1 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml1)};
            var yamlConfigSource2 =
                new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml2)};

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSource1);
            configurationBuilder.Add(yamlConfigSource2);
            var config = configurationBuilder.Build();

            Assert.Equal(4, config.GetSection("ip").GetChildren().Count());
            Assert.Equal("1.2.3.4", config["ip:0"]);
            Assert.Equal("7.8.9.10", config["ip:1"]);
            Assert.Equal("11.12.13.14", config["ip:2"]);
            Assert.Equal("15.16.17.18", config["ip:3"]);
        }

        [Fact]
        public void SequencesAreKeptInFileOrder()
        {
            var yaml = @"
            setting:
              - 'b'
              - 'a'
              - '2'
            ";

            var yamlConfigSource = new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml)};

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSource);
            var config = configurationBuilder.Build();

            var configurationSection = config.GetSection("setting");
            var indexConfigurationSections = configurationSection.GetChildren().ToArray();

            Assert.Equal(3, indexConfigurationSections.Count());
            Assert.Equal("b", indexConfigurationSections[0].Value);
            Assert.Equal("a", indexConfigurationSections[1].Value);
            Assert.Equal("2", indexConfigurationSections[2].Value);
        }

        [Fact]
        public void PropertiesAreSortedByNumberOnlyFirst()
        {
            var yaml = @"
            setting: 
              'hello': 'a'
              'bob': 'b'
              '42': 'c'
              '4': 'd'
              '10': 'e'
              '1text': 'f'
            ";

            var yamlConfigSource = new YamlFileConfigurationSource {FileProvider = InRamStreamUtil.StringToFileProvider(yaml)};

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.Add(yamlConfigSource);
            var config = configurationBuilder.Build();

            var configurationSection = config.GetSection("setting");
            var indexConfigurationSections = configurationSection.GetChildren().ToArray();

            Assert.Equal(6, indexConfigurationSections.Count());
            Assert.Equal("4", indexConfigurationSections[0].Key);
            Assert.Equal("10", indexConfigurationSections[1].Key);
            Assert.Equal("42", indexConfigurationSections[2].Key);
            Assert.Equal("1text", indexConfigurationSections[3].Key);
            Assert.Equal("bob", indexConfigurationSections[4].Key);
            Assert.Equal("hello", indexConfigurationSections[5].Key);
        }

        [Fact]
        public void FilesWithByteOrderMarkerAreParsedCorrectly()
        {
            var yaml = "setting1: '1'\nsetting2: '2'";

            var yamlConfigSource = new YamlConfigurationProvider(new YamlFileConfigurationSource());
            yamlConfigSource.Load(InRamStreamUtil.StringToStream(yaml, withBom: true));

            Assert.Equal("1", yamlConfigSource.Get("setting1"));
            Assert.Equal("2", yamlConfigSource.Get("setting2"));
        }
    }
}
