using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace CsprojCleaner
{
    public class WebFormsFileRemover : IFileRemover
    {
        private readonly string _fileExtension;

        public WebFormsFileRemover(string fileExtension)
        {
            _fileExtension = fileExtension;
        }
        
        public void CleanUpFiles(XmlDocument parsedXml)
        {
            var contentNodesFiles = GetContentIncludes(parsedXml);
            var compileNodesForContentNodes = GetCompileNodesRelatedToContentNodes(parsedXml, contentNodesFiles);
            var compileNodesWithoutContentNodes = GetCompileNodesNotRelatedToContentNodes(parsedXml, contentNodesFiles);

            foreach (var nodeToDelete in compileNodesForContentNodes)
            {
                nodeToDelete.ParentNode?.RemoveChild(nodeToDelete);
            }
            
            foreach (var nodeToDelete in contentNodesFiles)
            {
                nodeToDelete.ParentNode?.RemoveChild(nodeToDelete);
            }
            
            foreach (var nodeToDelete in compileNodesWithoutContentNodes)
            {
                nodeToDelete.ParentNode?.RemoveChild(nodeToDelete);
            }
        }
        
        private List<XmlNode> GetContentIncludes(XmlDocument parsedXml)
        {
            var results = new List<XmlNode>();

            foreach (XmlNode item in parsedXml.SelectNodes("Project/ItemGroup/Content"))
            {
                if (item.Attributes?["Include"] == null)
                {
                    continue;
                }
                var includeAttributeValue = item.Attributes["Include"].Value; 
                if (includeAttributeValue.EndsWithIgnoreCase($".{_fileExtension}"))
                {
                    results.Add(item);
                }
            }

            return results;
        }
        
        private List<XmlNode> GetCompileNodesRelatedToContentNodes(XmlDocument parsedXml, List<XmlNode> ascxFiles)
        {
            var results = new List<XmlNode>();

            foreach (XmlNode item in parsedXml.SelectNodes("Project/ItemGroup/Compile"))
            {
                if (item.Attributes?["Update"] == null)
                {
                    continue;
                }
                var updateAttributeValue = item.Attributes["Update"].Value;
                var ascxFileInUpdateAttribute = Regex.Replace(updateAttributeValue, $@"(.*\.{_fileExtension}).*", "$1");
                if (ascxFiles.Any(x => x.Attributes["Include"].Value.EqualsIgnoreCase(ascxFileInUpdateAttribute)))
                {
                    results.Add(item);
                }
            }

            return results;
        }
        
        private List<XmlNode> GetCompileNodesNotRelatedToContentNodes(XmlDocument parsedXml, List<XmlNode> ascxFiles)
        {
            var results = new List<XmlNode>();

            foreach (XmlNode item in parsedXml.SelectNodes("Project/ItemGroup/Compile"))
            {
                if (item.Attributes?["Update"] == null)
                {
                    continue;
                }

                if (!item.HasChildNodes)
                {
                    continue;
                }

                foreach (XmlNode childNode in item.ChildNodes)
                {
                    if (childNode.Name == "DependentUpon" && childNode.InnerText.ContainsIgnoreCase($".{_fileExtension}") && !ascxFiles.Any(x => x.Attributes["Include"].Value.EqualsIgnoreCase(childNode.InnerText)))
                    {
                        results.Add(item);
                    }
                }
            }

            return results;
        }
    }
}