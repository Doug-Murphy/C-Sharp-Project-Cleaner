using System.Xml;

namespace CsprojCleaner
{
    public interface IFileRemover
    {
        void CleanUpFiles(XmlDocument parsedXml);
    }
}