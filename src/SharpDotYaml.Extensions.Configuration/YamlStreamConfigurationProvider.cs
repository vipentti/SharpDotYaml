using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace SharpDotYaml.Extensions.Configuration
{
    /// <summary>
    /// A YAML file based on <see cref="StreamConfigurationProvider"/>.
    /// </summary>
    public class YamlStreamConfigurationProvider : StreamConfigurationProvider
    {
        /// <inheritdoc />
        public YamlStreamConfigurationProvider(StreamConfigurationSource source) : base(source)
        {
        }

        /// <inheritdoc />
        public override void Load(Stream stream)
        {
            try
            {
                Data = YamlConfigurationStreamParser.Parse(stream);
            }
            catch (YamlException e)
            {
                throw new FormatException(Strings.FormatError_YAMLParseError(e.Message), e);
            }
        }
    }
}
