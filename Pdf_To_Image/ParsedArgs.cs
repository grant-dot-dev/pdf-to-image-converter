namespace Pdf_To_Image;

public class ParsedArgs
{
    public string FullPath { get; set; }
    public string ProcessedOutDirectory { get; set; }
    public bool ProcessMultiplePages { get; set; }
    public bool ProcessToZipFile { get; set; }
}