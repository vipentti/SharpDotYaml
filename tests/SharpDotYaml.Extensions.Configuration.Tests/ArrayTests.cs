// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using Microsoft.Extensions.Configuration;
using static SharpDotYaml.Extensions.Configuration.Tests.ProviderUtils;

namespace SharpDotYaml.Extensions.Configuration.Tests;

public class ArrayTests
{
    [Fact]
    public void ArraysAreConvertedToKeyValuePairs()
    {
        var yaml = @"
        ip:
          - 1.2.3.4
          - 7.8.9.10
          - 11.12.13.14
        ";

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("ip:0").Should().Be("1.2.3.4");
        yamlConfiguration.Get("ip:1").Should().Be("7.8.9.10");
        yamlConfiguration.Get("ip:2").Should().Be("11.12.13.14");
    }

    [Fact]
    public void ArrayOfObjects()
    {
        var yaml = @"
        ip:
          - address: 1.2.3.4
            hidden: false
          - address: 7.8.9.10
            hidden: true
        ";

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("ip:0:address").Should().Be("1.2.3.4");
        yamlConfiguration.Get("ip:0:hidden").Should().Be("false");
        yamlConfiguration.Get("ip:1:address").Should().Be("7.8.9.10");
        yamlConfiguration.Get("ip:1:hidden").Should().Be("true");
    }

    [Fact]
    public void ImplicitArrayItemReplacement()
    {
        var yaml1 = @"
        ip:
          - 1.2.3.4
          - 7.8.9.10
          - 11.12.13.14
        ";

        var yaml2 = @"
        ip:
          - 15.16.17.18
        ";

        var yamlSource1 = GetYamlConfigurationSource(yaml1);
        var yamlSource2 = GetYamlConfigurationSource(yaml2);
        var config = new ConfigurationBuilder()
            .Add(yamlSource1)
            .Add(yamlSource2)
            .Build();

        config.GetSection("ip").GetChildren().Should().HaveCount(3);
        config["ip:0"].Should().Be("15.16.17.18");
        config["ip:1"].Should().Be("7.8.9.10");
        config["ip:2"].Should().Be("11.12.13.14");
    }

    [Fact]
    public void ExplicitArrayItemReplacement()
    {
        var yaml1 = @"
        ip:
          - 1.2.3.4
          - 7.8.9.10
          - 11.12.13.14
        ";

        var yaml2 = @"
        ip:
          1: 15.16.17.18
        ";

        var yamlSource1 = GetYamlConfigurationSource(yaml1);
        var yamlSource2 = GetYamlConfigurationSource(yaml2);
        var config = new ConfigurationBuilder()
            .Add(yamlSource1)
            .Add(yamlSource2)
            .Build();

        config.GetSection("ip").GetChildren().Should().HaveCount(3);
        config["ip:0"].Should().Be("1.2.3.4");
        config["ip:1"].Should().Be("15.16.17.18");
        config["ip:2"].Should().Be("11.12.13.14");
    }

    [Fact]
    public void MergesArrays()
    {
        var yaml1 = @"
        ip:
          - 1.2.3.4
          - 7.8.9.10
          - 11.12.13.14
        ";

        var yaml2 = @"
        ip:
          3: 15.16.17.18
        ";

        var yamlSource1 = GetYamlConfigurationSource(yaml1);
        var yamlSource2 = GetYamlConfigurationSource(yaml2);
        var config = new ConfigurationBuilder()
            .Add(yamlSource1)
            .Add(yamlSource2)
            .Build();

        config.GetSection("ip").GetChildren().Should().HaveCount(4);
        config["ip:0"].Should().Be("1.2.3.4");
        config["ip:1"].Should().Be("7.8.9.10");
        config["ip:2"].Should().Be("11.12.13.14");
        config["ip:3"].Should().Be("15.16.17.18");
    }

    [Fact]
    public void ArrayOrderingIsMaintained()
    {
        var yaml = @"
        ip:
          - b
          - a
          - 2
        ";

        var yamlSource = GetYamlConfigurationSource(yaml);

        var config = new ConfigurationBuilder()
            .Add(yamlSource)
            .Build();

        var section = config.GetSection("ip");
        var indexSections = section.GetChildren().ToArray();

        indexSections.Should().HaveCount(3);
        indexSections[0].Value.Should().Be("b");
        indexSections[1].Value.Should().Be("a");
        indexSections[2].Value.Should().Be("2");
    }

    [Fact]
    public void PropertiesAreSortedByNumberOnlyFirst()
    {
        var yaml = @"
        setting:
            hello: a
            bob: b
            42: c
            4: d
            10: e
            1text: f
        ";

        var yamlSource = GetYamlConfigurationSource(yaml);

        var config = new ConfigurationBuilder()
            .Add(yamlSource)
            .Build();

        var section = config.GetSection("setting");
        var indexSections = section.GetChildren().ToArray();

        indexSections.Should().HaveCount(6);
        indexSections[0].Key.Should().Be("4");
        indexSections[1].Key.Should().Be("10");
        indexSections[2].Key.Should().Be("42");
        indexSections[3].Key.Should().Be("1text");
        indexSections[4].Key.Should().Be("bob");
        indexSections[5].Key.Should().Be("hello");
    }
}
