using Microsoft.Extensions.Configuration;

namespace ComponentUILibrary.Models
{
    public class MasterModel
    {
        public string VirtualFolder()
        {
            try
            {
                string c = Directory.GetCurrentDirectory();
                IConfigurationRoot configuration =
                new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
                string VirtualFolder = configuration["SystemConfig:VirtualFolder"].ToString();

                return VirtualFolder;
            }
            catch (Exception)
            {

                return "~";
            }

        }

    }
}