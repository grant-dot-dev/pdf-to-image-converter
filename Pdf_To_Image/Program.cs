using System.IO.Compression;
using CommandLine;
using CommandLine.Text;
using Syncfusion.PdfToImageConverter;


namespace Pdf_To_Image
{
    class Program
    {
        private const string TempFolder = "_temp_zip";

        static void Main(string[] args)
        {
            // Parse arguments
            var parsedArgs = ParseArgs(args);

            using var imageConverter = new PdfToImageConverter();

            
            using var inputStream = new FileStream(parsedArgs.FullPath, FileMode.Open, FileAccess.Read);
            imageConverter.Load(inputStream);

            if (parsedArgs.ProcessMultiplePages)
            {
                // loop over All pages in document
                for (var i = 0; i < imageConverter.PageCount; i++)
                {
                    // Process each page
                    HandleSinglePage(imageConverter, parsedArgs.ProcessedOutDirectory, parsedArgs.ProcessToZipFile, i);
                }
            }
            else
            {
                // Process a single page
                HandleSinglePage(imageConverter, parsedArgs.ProcessedOutDirectory, parsedArgs.ProcessToZipFile);
            }

            // process images into zip file for easier sharing / organisation.
            if (parsedArgs.ProcessToZipFile)
            {
                ZipFiles(parsedArgs.ProcessedOutDirectory);
            }
        }

        
        //--- Helper Methods (these could be moved to separate files ---

        static void HandleSinglePage(PdfToImageConverter imageConverter, string outputPath,
            bool zip = false,
            int index = 0)
        {
            var outputStream = imageConverter.Convert(index, false, false);
            var stream = outputStream as MemoryStream ?? throw new InvalidOperationException();
            var imagePath = ConstructImagePath(outputPath, index, zip);

            var bytes = stream.ToArray();
            if (bytes is { Length: 0 })
            {
                Console.WriteLine("Failed to convert");
            }
            else
            {
                using var output = new FileStream(imagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                output.Write(bytes, 0, bytes.Length);
            }
        }

        static ParsedArgs ParseArgs(string[] args)
        {
            var fullFilePath = "";
            var inputFileDirectory = "";
            var outputPath = "";
            var multiPage = false;
            var zip = false;


            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<CliOptions>(args);

            parserResult
                .WithParsed(o =>
                {
                    fullFilePath = o.InputFileNamePath;
                    inputFileDirectory = Path.GetDirectoryName(fullFilePath);
                    outputPath = o.OutputDirectory ?? inputFileDirectory; // fallback to directory input file loaded
                    multiPage = o.MultiPage;
                    zip = o.Zip;
                })
                .WithNotParsed(_ =>
                {
                    var helpText = HelpText.AutoBuild(parserResult,
                        h => HelpText.DefaultParsingErrorsHandler(parserResult, h), e => e);
                    Console.WriteLine(helpText);
                    Environment.Exit(1);
                });


            var parsedOptions = new ParsedArgs()
            {
                FullPath = fullFilePath,
                ProcessMultiplePages = multiPage,
                ProcessedOutDirectory = outputPath,
                ProcessToZipFile = zip
            };

            return parsedOptions;
        }

        static string ConstructImagePath(string outputPath, int index, bool zip)
        {
            Console.WriteLine("-- Constructing Full Image Path -->");
            Console.WriteLine($"ProcessToZipFile -- {zip}");

            if (!zip)
            {
                return Path.Combine(outputPath, $"{Guid.NewGuid()}_page_{index}.png");
            }

            var tempFolderPath = Path.Combine(outputPath, TempFolder);

            // Ensure temp folder exists
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            return Path.Combine(tempFolderPath, $"{Guid.NewGuid()}_page_{index}.png");
        }

        static void ZipFiles(string originPath)
        {
            var tempPath = Path.Combine(originPath, TempFolder);

            // Ensure that the directory exists
            if (Directory.Exists(tempPath))
            {
                // Create the zip file
                ZipFile.CreateFromDirectory(tempPath, Path.Combine(originPath, $"images.zip"));
                Console.WriteLine("Files zipped successfully.");
            }
            else
            {
                Console.WriteLine("Directory not found.");
            }

            // Clean up temp folder
            Console.WriteLine("-----Clean up begun-----");
            Directory.Delete(tempPath, true);
            Console.WriteLine("-----Clean up complete-----");
        }
    }
}