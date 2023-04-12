// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using LibreConvert;
using LibreConvert.Documents;

Console.WriteLine("Starting conversion...");


Console.WriteLine("Enter the path to the document you want to convert: ");
var inputFilePath = Console.ReadLine() ?? throw new InvalidOperationException("Input file path is not valid");

inputFilePath = inputFilePath.Replace("\"", string.Empty);
var inputFileDirectory = Path.GetDirectoryName(inputFilePath)!;

var wordDocument = new WordDocument(inputFilePath);

Stopwatch stopwatch = new();
stopwatch.Start();
await wordDocument.ExportAsync(inputFileDirectory);
stopwatch.Stop();

Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
Console.WriteLine("Done!");