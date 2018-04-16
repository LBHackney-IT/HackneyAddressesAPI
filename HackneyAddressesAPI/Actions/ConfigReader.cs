using HackneyAddressesAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Actions
{
    public class ConfigReader : IConfigReader
    {
        private IConfigurationBuilder _configBuilder;
        private IConfiguration _config;

        public ConfigReader(string configFile, string directory)
        {
            _configBuilder = new ConfigurationBuilder()
           .SetBasePath(directory)
           .AddJsonFile(configFile);

            _config = _configBuilder.Build();
        }

        public ConfigReader()
        {
            _configBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            _config = _configBuilder.Build();
        }

        public object getConfigurationSetting(string settingName)
        {
           var value = _config[settingName];
           return value;
        }
    }
}
