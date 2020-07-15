using System;
using GranDen.Configuration.YamlData.Binder;
using GranDen.Configuration.YamlLoader;
using InRamStreamHelper;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace YamlDataBinderTest
{
    public class YamlDataFetchTest
    {
        class MissionData
        {
            public string ID { get; set; }
            public string Mission_Name { get; set; }
            public string Mission_Image_ID { get; set; }
            public int Mission_Type { get; set; }
            public int Mission_Style { get; set; }
            public int Mission_Element { get; set; }
            public string Mission_Scene_ID { get; set; }
            public string Mission_NPC_ID { get; set; }
            public int Mission_Enemy_Counts { get; set; }
            public int Mission_Min_Level { get; set; }
            public int Mission_Max_Level { get; set; }
            public string Mission_Reward_ID { get; set; }
            public int Mission_Max_Item_Get { get; set; }
            public int Mission_Stamina_Req { get; set; }
            public int Mission_Rarity { get; set; }
            public int Mission_Weight { get; set; }
            public int Mission_BonusWeight { get; set; }
            public DateTime? Mission_Bonus_Weight_Start_Time { get; set; }
            public DateTime? Mission_Bonus_Weight_End_Time { get; set; }
        }

        [Fact]
        public void TestFetchDataSheetUsingGetYamlData()
        {
            //Arrange

            #region yaml data

            var yamlStr = @"
Mission_MainData:
  Mission_000:
    ID: Mission_000
    Mission_Name: '[Mission_000_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 0
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_000
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_000
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 0
    Mission_Weight: 100
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_001:
    ID: Mission_001
    Mission_Name: '[Mission_001_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 1
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_001
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_001
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 0
    Mission_Weight: 80
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_002:
    ID: Mission_002
    Mission_Name: '[Mission_002_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 0
    Mission_Element: 2
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_002
    Mission_Enemy_Counts: 2
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_002
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 1
    Mission_Weight: 40
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
";

            #endregion

            var config = CreateConfiguration(yamlStr);

            //Act
            var missionData = config.GetYamlData<MissionData>("Mission_MainData", "Mission_002");

            //Assert
            Assert.Equal("Mission_002", missionData.ID);
            Assert.Equal("[Mission_002_Rescue_Cat]", missionData.Mission_Name);
            Assert.Equal("Mission_000_Image", missionData.Mission_Image_ID);
            Assert.Equal(0, missionData.Mission_Type);
            Assert.Equal(0, missionData.Mission_Style);
            Assert.Equal(2, missionData.Mission_Element);
            Assert.Equal("City_000", missionData.Mission_Scene_ID);
            Assert.Equal("Npc_Cat_002", missionData.Mission_NPC_ID);
            Assert.Equal(2, missionData.Mission_Enemy_Counts);
            Assert.Equal(3, missionData.Mission_Min_Level);
            Assert.Equal(2, missionData.Mission_Max_Level);
            Assert.Equal("Npc_Cat_002", missionData.Mission_Reward_ID);
            Assert.Equal(4, missionData.Mission_Max_Item_Get);
            Assert.Equal(1, missionData.Mission_Stamina_Req);
            Assert.Equal(1, missionData.Mission_Rarity);
            Assert.Equal(40, missionData.Mission_Weight);
            Assert.Equal(0, missionData.Mission_BonusWeight);
            Assert.Null(missionData.Mission_Bonus_Weight_Start_Time);
            Assert.Null(missionData.Mission_Bonus_Weight_End_Time);
        }

        [Fact]
        public void TestGetYamlCollectionData()
        {
            //Arrange

            #region yaml data

            var yamlStr = @"
Mission_MainData:
  Mission_000:
    ID: Mission_000
    Mission_Name: '[Mission_000_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 0
    Mission_Scene_ID: City_001
    Mission_NPC_ID: Npc_Cat_000
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_000
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 0
    Mission_Weight: 100
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_001:
    ID: Mission_001
    Mission_Name: '[Mission_001_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 1
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_001
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_001
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 0
    Mission_Weight: 80
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_002:
    ID: Mission_002
    Mission_Name: '[Mission_002_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 0
    Mission_Element: 2
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_002
    Mission_Enemy_Counts: 2
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_002
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 1
    Mission_Weight: 40
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_003:
    ID: Mission_003
    Mission_Name: '[Mission_000_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 0
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_003
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_003
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 1
    Mission_Weight: 20
    Mission_BonusWeight: 0
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_004:
    ID: Mission_004
    Mission_Name: '[Mission_000_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 1
    Mission_Element: 2
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_004
    Mission_Enemy_Counts: 1
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_004
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 2
    Mission_Weight: 10
    Mission_BonusWeight: 5
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
  Mission_005:
    ID: Mission_005
    Mission_Name: '[Mission_000_Rescue_Cat]'
    Mission_Image_ID: Mission_000_Image
    Mission_Type: 0
    Mission_Style: 0
    Mission_Element: 3
    Mission_Scene_ID: City_000
    Mission_NPC_ID: Npc_Cat_005
    Mission_Enemy_Counts: 2
    Mission_Min_Level: 3
    Mission_Max_Level: 2
    Mission_Reward_ID: Npc_Cat_005
    Mission_Max_Item_Get: 4
    Mission_Stamina_Req: 1
    Mission_Rarity: 2
    Mission_Weight: 5
    Mission_BonusWeight: 5
    Mission_Bonus_Weight_Start_Time: 
    Mission_Bonus_Weight_End_Time: 
";

            #endregion

            var config = CreateConfiguration(yamlStr);

            //Act
            var allMissionList = config.GetYamlCollectionData<MissionData>("Mission_MainData");
            var filteredMissionList =
                config.GetYamlCollectionData<MissionData>("Mission_MainData", "Mission_Element", new[] {"1", "2"});
            var queryMissionList =
                config.GetYamlCollectionData<MissionData>("Mission_MainData",
                    configuration => configuration.GetSection("Mission_Style").Value == "1");

            //Assert
            Assert.Equal(6, allMissionList.Count);
            Assert.Collection(allMissionList,
                m => Assert.Equal("Mission_000", m.ID),
                m => Assert.Equal("Mission_001", m.ID),
                m => Assert.Equal("Mission_002", m.ID),
                m => Assert.Equal("Mission_003", m.ID),
                m => Assert.Equal("Mission_004", m.ID),
                m => Assert.Equal("Mission_005", m.ID)
            );

            Assert.Equal(3, filteredMissionList.Count);
            Assert.Collection(filteredMissionList,
                m => Assert.Equal("Mission_001", m.ID),
                m => Assert.Equal("Mission_002", m.ID),
                m => Assert.Equal("Mission_004", m.ID)
            );

            Assert.Equal(4, queryMissionList.Count);
            Assert.Collection(queryMissionList,
                m => Assert.Equal("Mission_000", m.ID),
                m => Assert.Equal("Mission_001", m.ID),
                m => Assert.Equal("Mission_003", m.ID),
                m => Assert.Equal("Mission_004", m.ID)
            );
        }

        private static IConfigurationRoot CreateConfiguration(string rawString)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var yamlConfigSource =
                new YamlFileConfigurationSource() {FileProvider = InRamStreamUtil.StringToFileProvider(rawString)};
            configurationBuilder.Add(yamlConfigSource);
            var config = configurationBuilder.Build();
            return config;
        }
    }
}
