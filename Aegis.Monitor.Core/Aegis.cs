using System.Configuration;

namespace Aegis.Monitor.Core
{
    /// <summary>
    ///     <see cref="Aegis" /> provides helpful static methods for commonly used
    ///     tasks.
    /// </summary>
    public static class Aegis
    {
        /// <summary>
        ///     <see cref="IsEnabledInConfigFile" /> determines whether the application
        ///     configuration settings are correctly configured, and that Aegis is enabled
        ///     in the application configuration file.
        /// </summary>
        /// <param name="appSettingKey">
        ///     The application configuration key petaining to
        ///     Aegis start-mode.
        /// </param>
        /// <returns>
        ///     A <see cref="bool" /> value indicating whether or not Aegis is enabled
        ///     in the application configuration file.
        /// </returns>
        public static bool IsEnabledInConfigFile(string appSettingKey)
        {
            bool aegisIsEnabled;

            var settingsAreValid =
                bool.TryParse(
                    ConfigurationManager.AppSettings[appSettingKey],
                    out aegisIsEnabled);

            return settingsAreValid && aegisIsEnabled;
        }
    }
}