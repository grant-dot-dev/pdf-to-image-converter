# PDF to Image Converter

This tool will allow you to convert any PDF file into either a single or multiple .png files, dependent on the parameters passed in. 

# Getting Started

You can run the application in multiple ways:

*Option 1* 

Navigate to the _Archive.zip_ file and unzip the contents. Select the self-contained executable file suitable for your operating system (MacOS / Windows) - Linux is coming soon. 

*Option 2*
1. Clone the repo
2. Open the solution in the preferred editor / IDE
3. Navigate to the project folder
4. Run `dotnet run -i <inputPdfFilePath>`

Watch the magic unfold. 

## Usage

  -i, --inputFileNamePath    Required. Full file path and name of the file to be
                             converted

  -o, --outputDirectory      Directory path for output of processed files

  -m, --multiplePages        (Default: false) Should multiple pages be processed
                             into multiple images and zipped

  -z, --zip                  (Default: false) Build zip file from all images

  --help                     Display this help screen.
