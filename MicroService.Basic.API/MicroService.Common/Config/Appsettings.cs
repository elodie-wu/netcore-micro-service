using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MicroService.Common.Config
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }

        static Appsettings()
        {
            var Path = "appsettings.json";
            var directory = Directory.GetCurrentDirectory();

            Configuration = new
                ConfigurationBuilder().SetBasePath(directory).Add(new JsonConfigurationSource
                {
                    Path = Path,
                    Optional = false,
                    ReloadOnChange = true

                }).Build(); 
        }
         
        public static string Get(params string[] sections)
        {
            try
            { 
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception ex) { return "error:" + ex; } 
            return "";
        }
    }
}
