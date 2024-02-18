// Copyright 2023-2024 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using Microsoft.Extensions.Configuration;

namespace SharpDotYaml.Extensions.Configuration;

/// <summary>
/// A YAML file based on <see cref="FileConfigurationSource"/>.
/// </summary>
public class YamlConfigurationSource : FileConfigurationSource
{
    /// <inheritdoc />
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new YamlConfigurationProvider(this);
    }
}
