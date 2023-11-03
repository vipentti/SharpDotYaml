using Microsoft.Extensions.Configuration;

namespace SharpDotYaml.Extensions.Configuration
{
    /// <summary>
    /// A YAML file based on <see cref="StreamConfigurationSource"/>.
    /// </summary>
    public class YamlStreamConfigurationSource : StreamConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
            => new YamlStreamConfigurationProvider(this);
    }
}
