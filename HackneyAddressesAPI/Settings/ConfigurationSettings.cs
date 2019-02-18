using LBHAddressesAPI.Settings.Logging;

namespace LBHAddressesAPI.Settings
{
    /// <summary>
    /// Represents a POCO object of the values set as configuration in the appsettings.json
    /// Allowing for configuration values to be easily used and mocked
    /// </summary>
    public class ConfigurationSettings
    {
        
        /// <summary>
        /// Sentry error handling 
        /// </summary>
        public SentrySettings SentrySettings { get; set; }
    }
}
