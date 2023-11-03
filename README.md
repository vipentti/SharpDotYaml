![icon](https://raw.githubusercontent.com/vipentti/SharpDotYaml/main/icon.png)

# SharpDotYaml

SharpDotYaml is a C# library designed to support reading and loading YAML files as configuration providers.
The parsing is done by [YamlDotNet](https://github.com/aaubry/YamlDotNet)

## Features

- Load YAML files as configuration `Microsoft.Extensions.Configuration` providers

## Installation

To enable SharpDotYaml configuration provider install the NuGet package:

```sh
dotnet add package SharpDotYaml.Extensions.Configuration
```

## Usage with Microsoft.Extensions.Configuration

Here's a basic example of how to use SharpDotYaml with [Microsoft.Extensions.Configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration):

Install the NuGet package:

```sh
dotnet add package SharpDotYaml.Extensions.Configuration
```

Create a new file at the root of your project called `example.yaml` with the following contents:

```yaml
EXAMPLE_VALUE: this is only a test
ExampleSettings:
  Nested:
    Value: this is a nested value
```

Update the `.csproj` file for your project and add the following to include the file in the build directory:

```xml
  <ItemGroup>
    <None Include="example.yaml">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```

Read the configuration in your project:

```csharp
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddYamlFile("example.yaml")
    .Build();

// Read values from the configuration
Console.WriteLine("value: {0}, equals: {1}",
    config["EXAMPLE_VALUE"],
    config["EXAMPLE_VALUE"] == "this is only a test");

Console.WriteLine("value: {0}, equals: {1}",
    config["ExampleSettings:Nested:Value"],
    config["ExampleSettings:Nested:Value"] == "this is a nested value");
```

See also [examples/SharpDotYaml.ConfigurationExample](./examples/SharpDotYaml.ConfigurationExample)

## License

SharpDotYaml is licensed under the [MIT License](./LICENSE.md)
