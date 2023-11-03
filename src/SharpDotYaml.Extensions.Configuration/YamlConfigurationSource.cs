using Microsoft.Extensions.Configuration;

namespace SharpDotYaml.Extensions.Configuration
{
    /// <summary>
    /// A YAML file based on <see cref="FileConfigurationSource"/>.
    /// </summary>
    public class YamlConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new YamlConfigurationProvider(this);
        }
    }
}
