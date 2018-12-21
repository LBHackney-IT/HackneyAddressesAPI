using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Interfaces
{
    public interface IConfigReader
    {
        object getConfigurationSetting(string settingName);
    }
}
