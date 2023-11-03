using static SharpDotYaml.Extensions.Configuration.Tests.ProviderUtils;

namespace SharpDotYaml.Extensions.Configuration.Tests;

public class YamlConfigurationTests
{
    [Fact]
    public void CanLoadValidYaml()
    {
        var yaml = @"
        firstname: test
        test.last.name: last.name
        residential.address:
          street.name: something
          zipcode: 12345
        ";

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("firstname").Should().Be("test");
        yamlConfiguration.Get("test.last.name").Should().Be("last.name");
        yamlConfiguration.Get("residential.address:STREET.name").Should().Be("something");
        yamlConfiguration.Get("residential.address:zipcode").Should().Be("12345");
    }

    [Fact]
    public void CanLoadYamlWithAnchorsAndAliases()
    {
        var yaml = @"
            bill-to:  &id001
                street: 123 Tornado Alley Suite 16
                city:   East Westville
                state:  KS

            ship-to:  *id001
        ";

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("bill-to:street").Should().Be("123 Tornado Alley Suite 16");
        yamlConfiguration.Get("bill-to:city").Should().Be("East Westville");
        yamlConfiguration.Get("bill-to:state").Should().Be("KS");

        yamlConfiguration.Get("ship-to:street").Should().Be("123 Tornado Alley Suite 16");
        yamlConfiguration.Get("ship-to:city").Should().Be("East Westville");
        yamlConfiguration.Get("ship-to:state").Should().Be("KS");
    }

    [Fact]
    public void Throws_WhenUsingYamlMerge()
    {
        var yaml = @"
            bill-to:  &id001
                street: 123 Tornado Alley Suite 16
                city:   East Westville
                state:  KS

            ship-to:
                <<: *id001
                city: Override City
        ";

        var act = () => LoadYamlProvider(yaml);

        act.Should().ThrowExactly<FormatException>()
                .WithMessage("The YAML element has an invalid key: 'Merge is not supported'");
    }

    [Fact]
    public void CanLoadEmptyValue()
    {
        var yaml = @"
        firstname: ''
        ";

        var yamlConfiguration = LoadYamlProvider(yaml);

        yamlConfiguration.Get("firstname").Should().Be("");
    }

    [Fact]
    public void NonObjectRootIsInvalid()
    {
        var yaml = @"
        'firstname'
        ";

        var act = () => LoadYamlProvider(yaml);

        act.Should().ThrowExactly<FormatException>()
            .WithMessage("Top-level YAML element must be an object. Instead 'Scalar' was found.");
    }

    [Fact]
    public void CanLoadEmptyFile()
    {
        var yaml = "";

        var yamlConfiguration = LoadYamlProvider(yaml);
        yamlConfiguration.Should().NotBeNull();
    }

    public class YamlConfigurationProviderLoad
    {
        private readonly YamlConfigurationProvider src = new YamlConfigurationProvider(new YamlConfigurationSource());

        [Fact]
        public void Throws_WhenParsingInvalidYaml()
        {
            var yaml = @"
                invalid: invalid:
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Could not parse YAML:*");
        }

        [Fact]
        public void Throws_WhenParsingInvalidYamlObjectKey()
        {
            var yaml = @"
                {}: invalid
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Unsupported key element 'Mapping'*");
        }

        [Fact]
        public void Throws_WhenParsingEmptyKey()
        {
            var yaml = @"
                '': invalid
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("The YAML element has an invalid key: ''");
        }

        [Fact]
        public void Throws_WhenKeysAreDuplicated()
        {
            var yaml = @"
                invalid: 0
                invalid: 1
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Could not parse YAML: 'Duplicate key'.");
        }

        [Fact]
        public void Throws_WhenKeysAreDuplicatedCaseInsensitively()
        {
            var yaml = @"
                Invalid: 0
                invalid: 1
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("A duplicate key 'invalid' was found.");
        }
    }

    public class YamlStreamConfigurationProviderLoad
    {
        private readonly YamlStreamConfigurationProvider src = new YamlStreamConfigurationProvider(new YamlStreamConfigurationSource());

        [Fact]
        public void Throws_WhenParsingInvalidYaml()
        {
            var yaml = @"
                invalid: invalid:
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Could not parse YAML:*");
        }

        [Fact]
        public void Throws_WhenParsingInvalidYamlObjectKey()
        {
            var yaml = @"
                {}: invalid
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Unsupported key element 'Mapping'*");
        }

        [Fact]
        public void Throws_WhenParsingEmptyKey()
        {
            var yaml = @"
                '': invalid
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("The YAML element has an invalid key: ''");
        }

        [Fact]
        public void Throws_WhenKeysAreDuplicated()
        {
            var yaml = @"
                invalid: 0
                invalid: 1
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("Could not parse YAML: 'Duplicate key'.");
        }

        [Fact]
        public void Throws_WhenKeysAreDuplicatedCaseInsensitively()
        {
            var yaml = @"
                Invalid: 0
                invalid: 1
            ";

            var act = () => src.Load(yaml.StringToStream());

            act.Should().ThrowExactly<FormatException>()
                .WithMessage("A duplicate key 'invalid' was found.");
        }
    }
}
