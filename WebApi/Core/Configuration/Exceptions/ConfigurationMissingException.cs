namespace WebApi.Core.Configuration.Exceptions;

[Serializable]
public class ConfigurationMissingException : InvalidOperationException
{
    public ConfigurationMissingException(string key, string section) : base(
        $"Configuration key '{key}' from section '{section}' is missing.")
    {
    }
}