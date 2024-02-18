// Copyright 2023-2024 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace SharpDotYaml.Extensions.Configuration;

/// <summary>
/// A YAML file based on <see cref="FileConfigurationProvider"/>.
/// </summary>
public class YamlConfigurationProvider : FileConfigurationProvider
{
    /// <summary>
    /// Initialize new instance with the given source
    /// </summary>
    /// <param name="source">The source</param>
    public YamlConfigurationProvider(YamlConfigurationSource source)
        : base(source) { }

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
