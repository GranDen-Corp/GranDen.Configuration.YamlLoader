using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GranDen.YamlLoader;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VerifyYamlLoaderWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(builder =>
                   {
                       var fileList = new DirectoryInfo("YamlData").GetFiles("*.yaml", SearchOption.AllDirectories);
                       foreach (var f in fileList)
                       {
                           builder.AddYamlFile(f.FullName, optional: true, reloadOnChange: true);
                       }

                       builder.Build();
                   })
                .UseStartup<Startup>();
    }
}
