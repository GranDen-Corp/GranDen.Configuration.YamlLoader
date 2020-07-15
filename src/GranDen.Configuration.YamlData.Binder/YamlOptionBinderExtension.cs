using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace GranDen.Configuration.YamlData.Binder
{
    /// <summary>
    /// Extension methods for easier access data in Yaml Configuration
    /// </summary>
    public static class YamlOptionBinderExtension
    {
        /// <summary>
        /// Get Typed option instance
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetTypedOption<T>(this IConfiguration configuration, string key) where T : new()
        {
            return configuration.GetSection(key).Get<T>();
        }

        /// <summary>
        /// Get Typed option class instance form configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetYamlData<T>(this IConfiguration configuration, string sheetName, string id) where T : new()
        {
            var section = configuration.GetSection(sheetName).GetSection(id);

            if (!section.Exists())
            {
                throw new Exception($"Sheet: '{sheetName}', ID: '{id}' section not exist in this configuration.");
            }

            var ret = new T();
            section.Bind(ret);
            return ret;
        }

        /// <summary>
        ///  Get Typed option instance collection  
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<T> GetYamlCollectionData<T>(this IConfiguration configuration, string sheetName)
            where T : new()
        {
            var section = configuration.GetSection(sheetName);

            return section.Get<List<T>>() ?? new List<T>();
        }

        /// <summary>
        /// Conditionally get Typed option instance collection
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <param name="colName"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<T> GetYamlCollectionData<T>(this IConfiguration configuration, string sheetName,
            string colName, string condition) where T : new()
        {
            var configurationSections = configuration.QuerySheet(sheetName)
                .Where(x => x.GetSection(colName).Value == condition);

            var ret = new List<T>();
            foreach (var configurationSection in configurationSections)
            {
                var obj = new T();
                configurationSection.Bind(obj);
                ret.Add(obj);
            }

            return ret;
        }

        /// <summary>
        /// Conditionally get Typed option instance collection
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <param name="colName"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetYamlCollectionData<T>(this IConfiguration configuration, string sheetName,
            string colName, IEnumerable<string> conditions) where T : new()
        {
            var retList = new List<T>();
            foreach (var condition in conditions)
            {
                var list = configuration.GetYamlCollectionData<T>(sheetName, colName, condition);
                retList.AddRange(list);
            }

            return retList;
        }

        /// <summary>
        /// Get Typed option instance collection using query func
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <param name="conditionFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetYamlCollectionData<T>(this IConfiguration configuration, string sheetName,
            Func<IConfigurationSection, bool> conditionFunc) where T : new()
        {
            var configurationSections = configuration.QuerySheet(sheetName).Where(conditionFunc);

            var ret = new List<T>();
            foreach (var configurationSection in configurationSections)
            {
                var obj = new T();
                configurationSection.Bind(obj);
                ret.Add(obj);
            }

            return ret;
        }

        private static IEnumerable<IConfigurationSection> QuerySheet(this IConfiguration configuration, string sheetName)
        {
            return configuration.GetSection(sheetName).GetChildren();
        }

        /// <summary>
        /// Get string list stored in configuration that read from YAML
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sheetName"></param>
        /// <param name="colName"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<string> GetYamlStringListData(this IConfiguration configuration, string sheetName,
            string colName,
            string condition)
        {
            var configurationSections =
                configuration.QuerySheet(sheetName).Where(x => x.GetSection(colName).Value == condition);

            return configurationSections.Select(configurationSection => configurationSection.Key).ToList();
        }
    }
}
