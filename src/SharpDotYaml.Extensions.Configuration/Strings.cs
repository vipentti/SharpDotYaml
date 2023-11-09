// Copyright 2023 Ville Penttinen
// Distributed under the MIT License.
// https://github.com/vipentti/SharpDotYaml/blob/main/LICENSE.md

namespace SharpDotYaml.Extensions.Configuration
{
    internal static class Strings
    {
        public const string Error_InvalidTopLevelYAMLElement = "Top-level YAML element must be an object. Instead '{0}' was found.";
        public const string Error_UnsupportedKeyElement = "Unsupported key element '{0}' {1}.";
        public const string Error_UnsupportedYAMLElement = "Unsupported YAML element '{0}' {1}.";
        public const string Error_KeyIsDuplicated = "A duplicate key '{0}' was found.";
        public const string Error_YAMLParseError = "Could not parse YAML: '{0}'.";
        public const string Error_InvalidPath = "The path provided was invalid.";
        public const string Error_InvalidKey = "The YAML element has an invalid key: '{0}'";

        public static string FormatError_InvalidTopLevelYAMLElement(object arg0) => string.Format(Error_InvalidTopLevelYAMLElement, arg0);
        public static string FormatError_UnsupportedKeyElement(object arg0, object arg1) => string.Format(Error_UnsupportedKeyElement, arg0, arg1);
        public static string FormatError_UnsupportedYAMLElement(object arg0, object arg1) => string.Format(Error_UnsupportedYAMLElement, arg0, arg1);
        public static string FormatError_KeyIsDuplicated(object arg0) => string.Format(Error_KeyIsDuplicated, arg0);
        public static string FormatError_YAMLParseError(object arg0) => string.Format(Error_YAMLParseError, arg0);
        public static string FormatError_InvalidKey(object arg0) => string.Format(Error_InvalidKey, arg0);
    }
}
