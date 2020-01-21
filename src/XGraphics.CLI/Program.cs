using System.Collections.Generic;
using System.IO;
using CommandLine;
using XGraphics.Tools.XGraphicsExport;
using XGraphics.StandardModel;

namespace XGraphics.CLI
{
    class Program
    {
        [Verb("convert2xg", HelpText = "Convert .svg file(s) to .xg (XGraphics)")]
        class ConvertToXgOptions
        {
            [Option('v', "verbose", Default = false, Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Option('f', "files", Required = true, HelpText = "Input files to be processed.")]
            public IEnumerable<string> InputFiles { get; set; }
        }

        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<ConvertToXgOptions>(args)
                .MapResult(
                    ConvertToXg,
                    errs => 1);
        }

        private static int ConvertToXg(ConvertToXgOptions convertToXgOptions)
        {
            foreach (string inputFileName in convertToXgOptions.InputFiles)
            {
                SvgImporter.SvgImporter svgImporter = new SvgImporter.SvgImporter();

                using FileStream inputStream = File.Open(inputFileName, FileMode.Open);
                XCanvas xCanvas = svgImporter.Import(inputStream);

                string outputFileName = Path.ChangeExtension(inputFileName, ".xg");

                using FileStream outputStream = File.Open(outputFileName, FileMode.Create);
                new XGraphicsExporter(xCanvas).Export(outputStream);
            }

            return 0;
        }
    }
}
