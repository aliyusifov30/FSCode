using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Persistence
{
    public static class Configuration
    {
        public static string GetConnectionString { 
            get
            {
                ConfigurationManager manager = new();
                manager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/FSCode.Api"));
                manager.AddJsonFile("appsettings.json");

                return manager.GetConnectionString("FSCodeDb");
            }
        }
    }
}
