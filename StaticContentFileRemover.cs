using System.Collections.Generic;
using System.Xml;

namespace CsprojCleaner
{
    public class StaticContentFileRemover : IFileRemover
    {
        private readonly string _fileExtension;

        public StaticContentFileRemover(string fileExtension)
        {
            _fileExtension = fileExtension;
        }
        
        public void CleanUpFiles(XmlDocument parsedXml)
        {
            var nodes = GetContentIncludes(parsedXml);

            foreach (var nodeToDelete in nodes)
            {
                if (nodeToDelete.ParentNode == null)
                {
                    continue;
                }

                nodeToDelete.ParentNode.RemoveChild(nodeToDelete);
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
    }
}