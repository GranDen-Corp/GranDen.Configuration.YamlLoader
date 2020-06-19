using System.IO;
using GranDen.Configuration.YamlLoader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace VerifyYamlLoaderWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(builder =>
                   {
                       var fileList = new DirectoryInfo("YamlData").GetFiles("*.yaml", SearchOption.AllDirectories);
                       foreach (var f in fileList)
                       {
                           builder.AddYamlFile(f.FullName, optional: true, reloadOnChange: true);
                       }
                   })
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                        webBuilder.UseStartup<Startup>();
                   });
    }
}
