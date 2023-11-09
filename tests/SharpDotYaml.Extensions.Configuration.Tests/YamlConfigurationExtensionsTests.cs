// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using Microsoft.Extensions.Configuration;

namespace SharpDotYaml.Extensions.Configuration.Tests;

public static class YamlConfigurationExtensionsTests
{
    public class AddYamlFile
    {
        private readonly ConfigurationBuilder builder = new ConfigurationBuilder();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Throws_IfFilePathIsNullOrEmpty(string? path)
        {
            var act = () => YamlConfigurationExtensions.AddYamlFile(builder, path!);

            act.Should().ThrowExactly<ArgumentException>()
                .Which.ParamName.Should().Be(nameof(path))
            ;
        }
        [Fact]
        public void Throws_IfFileDoesNotExistAndIsNotOptional()
        {
            var path = "file-does-not-exist.yaml";
            var act = () => builder.AddYamlFile(path, optional: false).Build();
            act.Should().ThrowExactly<FileNotFoundException>()
                .WithMessage($"The configuration file '{path}' was not found and is not optional.*");
        }

        [Fact]
        public void DoesNotThrow_IfFileDoesNotExistAndIsOptional()
        {
            var act = () => builder.AddYamlFile("file-does-not-exist.yaml", optional: true).Build();
            act.Should().NotThrow();
        }

        [Fact]
        public void SupportsLoadingDataFromFileProvider()
        {
            var env = """
            test: value
            """;

            builder.AddYamlFile(provider: env.StringToFileProvider(), "file-does-not-exist.yaml", optional: false, reloadOnChange: false);

            var config = builder.Build();
            config["test"].Should().Be("value");
        }
    }

    public class AddYamlStream
    {
        private readonly ConfigurationBuilder builder = new ConfigurationBuilder();

        [Fact]
        public void Throws_IfStreamIsNull()
        {
            var act = () => builder.AddYamlStream(null!).Build();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Source.Stream cannot be null.");
        }

        [Fact]
        public void SupportsLoadingDataFromStream()
        {
            var env = """
            test: value
            """;

            var config = builder.AddYamlStream(env.StringToStream()).Build();
            config["test"].Should().Be("value");
        }

        [Fact]
        public void Throws_IfReadingMultipleTimesFromStream()
        {
            var env = """
            test: value
            """;

            _ = builder.AddYamlStream(env.StringToStream()).Build();

            var act = () => builder.Build();

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Stream was not readable.");
        }

        [Fact]
        public void Throws_IfStreamWasDisposed()
        {
            var env = """
            test: value
            """;
            using (var stream = env.StringToStream())
            {
                builder.AddYamlStream(stream);
            }

            var act = () => builder.Build();

            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage("Stream was not readable.");
        }

        [Fact]
        public void Throws_WhenReloading()
        {
            var env = """
            test: value
            """;

            var config = builder.AddYamlStream(env.StringToStream()).Build();

            var act = () => config.Reload();

            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("StreamConfigurationProviders cannot be loaded more than once.");
        }
    }
}
