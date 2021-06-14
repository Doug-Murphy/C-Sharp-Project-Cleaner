using System;
using System.Xml;

namespace CsprojCleaner {
    class Program {
        static int Main(string[] args) {
            if (args.Length != 1 || string.IsNullOrWhiteSpace(args[0])) {
                Console.WriteLine("You must specify an argument with the absolute path to the .csproj file.");
                return 1;
            }

            string csProjPath = args[0].Trim('"');

            var parsedXml = new XmlDocument();
            parsedXml.Load(csProjPath);

            new StaticContentFileRemover("bmp").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("css").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("eot").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("gif").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("html").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("ico").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("jpg").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("js").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("js.map").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("json").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("md").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("png").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("scss").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("svg").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("ts").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("ttf").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("woff").CleanUpFiles(parsedXml);
            new StaticContentFileRemover("woff2").CleanUpFiles(parsedXml);

            new WebFormsFileRemover("asax").CleanUpFiles(parsedXml);
            new WebFormsFileRemover("ascx").CleanUpFiles(parsedXml);
            new WebFormsFileRemover("aspx").CleanUpFiles(parsedXml);
            new WebFormsFileRemover("master").CleanUpFiles(parsedXml);

            parsedXml.Save(csProjPath);

            return 0;
        }
    }
}