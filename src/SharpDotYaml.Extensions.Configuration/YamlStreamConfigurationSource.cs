// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using Microsoft.Extensions.Configuration;

namespace SharpDotYaml.Extensions.Configuration;

/// <summary>
/// A YAML file based on <see cref="StreamConfigurationSource"/>.
/// </summary>
public class YamlStreamConfigurationSource : StreamConfigurationSource
{
    /// <inheritdoc />
    public override IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new YamlStreamConfigurationProvider(this);
}
