using CommandLine;

namespace Pdf_To_Image;

public class CliOptions
{
    [Option('i', "inputFileNamePath", Required = true, HelpText = "Full file path and name of file to be converted")]
    public string InputFileNamePath { get; set; }

    [Option('o', "outputDirectory", Required = false,
        HelpText = "Directory path for output of processed files")]
    public string? OutputDirectory { get; set; }

    [Option('m', "multiplePages", Required = false, Default = false,
        HelpText = "Should multiple pages be processed into multiple images and zipped")]
    public bool MultiPage { get; set; }

    [Option('z', "zip", Required = false, Default = false,
        HelpText = "Build zip file from all images")]
    public bool Zip { get; set; }

   
}