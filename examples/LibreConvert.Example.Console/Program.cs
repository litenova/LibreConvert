// See https://aka.ms/new-console-template for more information

using LibreConvert.Connection;
using LibreConvert.Connection.Configuration;

var connection = new LibreOfficeConnection(new LibreOfficeConnectionConfiguration
{
    Communication = new LibreOfficeConnectionPipeCommunication()
});

Console.WriteLine("Enter the Path to Input File: ");
string inputFilePath = Console.ReadLine() ?? throw new InvalidOperationException();

Console.WriteLine("Enter the Path to Output File: ");
string outputFilePath = Console.ReadLine() ?? throw new InvalidOperationException();

Console.ReadLine();