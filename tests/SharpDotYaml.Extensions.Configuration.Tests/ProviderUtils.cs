﻿namespace SharpDotYaml.Extensions.Configuration.Tests;

public static class ProviderUtils
{
    public static YamlConfigurationProvider LoadYamlProvider(string input)
    {
        var src = new YamlConfigurationProvider(new YamlConfigurationSource());
        src.Load(input.StringToStream());
        return src;
    }

    public static YamlConfigurationSource GetYamlConfigurationSource(string input) =>
        new YamlConfigurationSource { FileProvider = input.StringToFileProvider() };
}
