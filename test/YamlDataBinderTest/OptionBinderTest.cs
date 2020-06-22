using System.Collections.Generic;
using GranDen.Configuration.YamlData.Binder;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YamlDataBinderTest
{
    public class OptionBinderTest
    {
        class PositionOptions
        {
            public string Title { get; set; }
            public string Name { get; set; }
        }
        
        [Fact]
        public void TestBindTypedOptionClassObject()
        {
            //Arrange
            var builder = new ConfigurationBuilder();
            var inRamDict = new Dictionary<string, string>()
            {
                {"Position:Title", "Dictionary_Title"},
                {"Position:Name", "Dictionary_Name" },
            };
            builder.AddInMemoryCollection(inRamDict);

            var configuration = builder.Build();
            
            //Act
            var obj = configuration.GetTypedOption<PositionOptions>("Position");
            
            //Assert
            Assert.Equal("Dictionary_Title", obj.Title);
            Assert.Equal("Dictionary_Name", obj.Name);
        }
    }
}