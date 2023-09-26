using System.Text;
using System.Xml.Linq;

namespace Fb2CoverCutter
{
    internal class Fb2CoverCutter
    {
        private readonly string _inputFile;
        private readonly string _outputFile;
        private readonly bool _quiet;

        public Fb2CoverCutter(string inputFile, string outputFile, bool quiet)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
            _quiet = quiet;
        }

        public void Execute()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Log("Reading file '{0}'", _inputFile);
                var doc = XDocument.Load(_inputFile);

                var coverpageElement = doc.Descendants().FirstOrDefault(t => t.Name.LocalName == "coverpage");

                if (coverpageElement == null)
                {
                    Log("<coverpage> element not found");
                    return;
                }

                var imageElement = coverpageElement.Descendants().FirstOrDefault(t => t.Name.LocalName == "image");

                if (imageElement == null)
                {
                    Log("<image> element not found inside <coverpage> element");
                    return;
                }

                var hrefAttr = imageElement.Attributes().FirstOrDefault(t => t.Name.LocalName == "href");

                if (hrefAttr == null)
                {
                    Log("<image> element has no 'href' attribute");
                    return;
                }

                var href = hrefAttr.Value;
                var id = href.Replace("#", "");

                var binaryElement = doc.Descendants().FirstOrDefault(t => t.Name.LocalName == "binary" && t.Attribute("id")?.Value == id);

                if (binaryElement == null)
                {
                    Log("<binary> element with id '{0}' not found", id);
                    return;
                }

                Log("Removing <coverpage> and <binary> elements");

                binaryElement.Remove();
                coverpageElement.Remove();

                Log("Writing file '{0}'", _outputFile);

                doc.Save(_outputFile);
            }
            catch (Exception exception)
            {
                Log(exception.ToString());
            }
        }

        private void Log(string format, params object[] args)
        {
            if (!_quiet)
                Console.WriteLine(format, args);
        }
    }
}
