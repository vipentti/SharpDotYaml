// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace SharpDotYaml.Extensions.Configuration;

/// <summary>
/// Parses YAML from a stream into configuration dictionary
/// </summary>
internal sealed class YamlConfigurationStreamParser
{
    private readonly IDictionary<string, string?> _data = new Dictionary<string, string?>(
        StringComparer.OrdinalIgnoreCase
    );
    private readonly Stack<string> _context = new();

    public static IDictionary<string, string?> Parse(Stream input) =>
        new YamlConfigurationStreamParser().ParseStream(input);

    private IDictionary<string, string?> ParseStream(Stream input)
    {
        using (var reader = new StreamReader(input, detectEncodingFromByteOrderMarks: true))
        {
            var yaml = new YamlStream();
            yaml.Load(reader);

            foreach (var doc in yaml.Documents)
            {
                if (doc.RootNode is not YamlMappingNode mapping)
                {
                    throw new FormatException(
                        Strings.FormatError_InvalidTopLevelYAMLElement(doc.RootNode.NodeType)
                    );
                }

                VisitYamlMappingNode(mapping);
            }

            return _data;
        }
    }

    private void VisitYamlMappingNode(YamlMappingNode node)
    {
        var isEmpty = true;

        foreach (var yamlNodePair in node.Children)
        {
            isEmpty = false;
            EnterContext(GetKeyValue(yamlNodePair.Key));
            VisitYamlNode(yamlNodePair.Value);
            ExitContext();
        }

        if (isEmpty && _context.Count > 0)
        {
            _data[_context.Peek()] = null;
        }

        static string GetKeyValue(YamlNode node)
        {
            if (node is not YamlScalarNode scalarNode)
            {
                throw new FormatException(
                    Strings.FormatError_UnsupportedKeyElement(node.NodeType, node.Start)
                );
            }

            var context = scalarNode.Value;

            if (string.IsNullOrEmpty(context))
            {
                throw new FormatException(Strings.FormatError_InvalidKey(context ?? "(null)"));
            }

            // Currently YamlStream does not support Merges
            // https://github.com/aaubry/YamlDotNet/issues/388
            if (context == "<<")
            {
                throw new FormatException(Strings.FormatError_InvalidKey("Merge is not supported"));
            }

            return context ?? "";
        }
    }

    private void VisitYamlNode(YamlNode node)
    {
        Debug.Assert(_context.Count > 0);

        switch (node)
        {
            case YamlMappingNode mapping:
                VisitYamlMappingNode(mapping);
                break;

            case YamlSequenceNode yamlSequence:
                for (var i = 0; i < yamlSequence.Children.Count; i++)
                {
                    EnterContext(i.ToString());
                    VisitYamlNode(yamlSequence.Children[i]);
                    ExitContext();
                }
                break;
            case YamlScalarNode yamlScalar:
                var currentKey = _context.Peek();

                if (_data.ContainsKey(currentKey))
                {
                    throw new FormatException(Strings.FormatError_KeyIsDuplicated(currentKey));
                }

                _data[currentKey] = IsNullValue(yamlScalar) ? null : yamlScalar.Value;
                break;

            default:
                throw new FormatException(
                    Strings.FormatError_UnsupportedYAMLElement(node.NodeType, node.Start)
                );
        }

        static bool IsNullValue(YamlScalarNode yamlValue)
        {
            return yamlValue.Style == ScalarStyle.Plain
                && (
                    yamlValue.Value == "~"
                    || yamlValue.Value == "null"
                    || yamlValue.Value == "Null"
                    || yamlValue.Value == "NULL"
                );
        }
    }

    private void EnterContext(string context) =>
        _context.Push(
            _context.Count > 0
                ? $"{_context.Peek()}{ConfigurationPath.KeyDelimiter}{context}"
                : context
        );

    private void ExitContext() => _context.Pop();
}
