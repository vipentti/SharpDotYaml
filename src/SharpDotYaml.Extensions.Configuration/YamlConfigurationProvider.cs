﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using YamlDotNet.Core;

namespace SharpDotYaml.Extensions.Configuration
{
    /// <summary>
    /// A YAML file based on <see cref="FileConfigurationProvider"/>.
    /// </summary>
    public class YamlConfigurationProvider : FileConfigurationProvider
    {
        /// <summary>
        /// Initialize new instance with the given source
        /// </summary>
        /// <param name="source">The source</param>
        public YamlConfigurationProvider(YamlConfigurationSource source) : base(source) { }

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
