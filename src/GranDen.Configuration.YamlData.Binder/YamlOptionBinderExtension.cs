using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace GranDen.Configuration.YamlData.Binder
{
    /// <summary>
    /// Extension methods for easier access data in Yaml Configuration
    /// </summary>
    public static class YamlOptionBinderExtension
    {
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
        public static List<T> GetYamlCollectionData<T>(this IConfiguration configuration, string sheetName) where T : new()
        {
            var section = configuration.GetSection(sheetName);

            if (!section.Exists())
            {
                throw new Exception($"SheetName: {sheetName} not exist.");
            }

            return section.Get<List<T>>(); 
        }
    }
}