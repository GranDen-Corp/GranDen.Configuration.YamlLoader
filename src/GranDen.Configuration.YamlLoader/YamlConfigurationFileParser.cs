using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace GranDen.Configuration.YamlLoader
{
    internal class YamlConfigurationFileParser
    {
        private readonly IDictionary<string, string> _data =
            new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        public IDictionary<string, string> Parse(Stream input)
        {
            _data.Clear();
            _context.Clear();

            // https://dotnetfiddle.net/rrR2Bb
            var yaml = new YamlStream();
            var reader = new StreamReader(input, detectEncodingFromByteOrderMarks: true);

            try
            {
                yaml.Load(reader);
            }
            //NOTE: On some siutation, YAML parser in YamlDotNet 8.x doesn't throw parsing error exception correctly. 
            catch (Exception ex) when (!(ex is YamlException))
            {
                throw new YamlException("YAML file parse error", ex);
            }

            if (!yaml.Documents.Any())
            {
                return _data;
            }

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            // The document node is a mapping node
            VisitYamlMappingNode(mapping);

            return _data;
        }

        private void VisitYamlNodePair(KeyValuePair<YamlNode, YamlNode> yamlNodePair)
        {
            var context = ((YamlScalarNode)yamlNodePair.Key).Value;
            VisitYamlNode(context, yamlNodePair.Value);
        }

        private void VisitYamlNode(string context, YamlNode node)
        {
            switch (node)
            {
                case YamlScalarNode _:
                    VisitYamlScalarNode(context, (YamlScalarNode)node);
                    break;
                case YamlMappingNode _:
                    VisitYamlMappingNode(context, (YamlMappingNode)node);
                    break;
                case YamlSequenceNode _:
                    VisitYamlSequenceNode(context, (YamlSequenceNode)node);
                    break;
            }
        }

        private void VisitYamlScalarNode(string context, YamlScalarNode yamlValue)
        {
            //a node with a single 1-1 mapping 
            EnterContext(context);
            var currentKey = _currentPath;

            if (_data.ContainsKey(currentKey))
            {
                throw new FormatException($"A duplicate key '{currentKey}' founded");
            }

            _data[currentKey] = yamlValue.Value;
            ExitContext();
        }

        private void VisitYamlMappingNode(YamlMappingNode node)
        {
            foreach (var yamlNodePair in node.Children)
            {
                VisitYamlNodePair(yamlNodePair);
            }
        }

        private void VisitYamlMappingNode(string context, YamlMappingNode yamlValue)
        {
            //a node with an associated sub-document
            EnterContext(context);

            VisitYamlMappingNode(yamlValue);

            ExitContext();
        }

        private void VisitYamlSequenceNode(string context, YamlSequenceNode yamlValue)
        {
            //a node with an associated list
            EnterContext(context);

            VisitYamlSequenceNode(yamlValue);

            ExitContext();
        }

        private void VisitYamlSequenceNode(YamlSequenceNode node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                VisitYamlNode(i.ToString(), node.Children[i]);
            }
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
