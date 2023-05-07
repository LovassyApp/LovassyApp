namespace WebApi.Common.Exceptions;

/// <summary>
///     The exception thrown when a required configuration key is missing. Has to be manually thrown in the controller of
///     whatever service is using the configuration.
/// </summary>
[Serializable]
public class ConfigurationMissingException : InvalidOperationException
{
    public ConfigurationMissingException(string key, string section) : base(
        $"Configuration key '{key}' from section '{section}' is missing.")
    {
    }
}