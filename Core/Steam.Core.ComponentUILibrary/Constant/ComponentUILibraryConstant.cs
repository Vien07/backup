using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Steam.Core.ComponentUILibrary.Constant
{

    public class ComponentUILibraryConstant
    {

        public static readonly string VirtualFolder;
        public static readonly string MedidaFileServer;
        public static readonly string DefaultConnectionDatabase;

        // Static constructor
        static ComponentUILibraryConstant()
        {
            IConfiguration conf = (new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)  
                     .Build());
            VirtualFolder = InitializeValue(conf, "SystemConfig:VirtualFolder");
            MedidaFileServer = InitializeValue(conf, "SystemConfig:MedidaFileServer");
            DefaultConnectionDatabase = InitializeValue(conf, "SystemConfig:DefaultConnectionDatabase");
        }

        private static string InitializeValue(IConfiguration conf,string key)
        {
            try
            {
                string value = conf[key];

                return value;
            }
            catch (Exception)
            {

               return string.Empty;
            }
       
        }
    }

}
