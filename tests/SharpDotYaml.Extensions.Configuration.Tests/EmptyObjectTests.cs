// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using static SharpDotYaml.Extensions.Configuration.Tests.ProviderUtils;

namespace SharpDotYaml.Extensions.Configuration.Tests;

public class EmptyObjectTests
{
    [Fact]
    public void EmptyObject_AddsAsNull()
    {
        var yaml = """
        key: { }
        """;

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("key").Should().BeNull();
    }

    [Theory]
    [InlineData("~")]
    [InlineData("null")]
    [InlineData("Null")]
    [InlineData("NULL")]
    public void NullObject_AddsAsNull(string nullValue)
    {
        var yaml = $"""
        key: {nullValue}
        """;

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("key").Should().BeNull();
    }

    [Fact]
    public void NestedObject_DoesNotAddParent()
    {
        var yaml = """
        key:
          nested: value
        """;

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.TryGet("key", out _).Should().BeFalse();
        yamlConfiguration.Get("key:nested").Should().Be("value");
    }
}
