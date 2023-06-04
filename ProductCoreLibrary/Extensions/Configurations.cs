using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCoreLibrary.Extensions
{
    public static class Configurations
    {
        public static string GetAppsettings(string config)
        {
            if (String.IsNullOrEmpty(config))
                throw new ArgumentException($"Config: {config} not found");
            return config;
        }
    }
}
