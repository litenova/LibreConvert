namespace LibreConvert.Client;

public interface ILibreOfficeClient
{
    void Convert(string inputFile, string outputFile);
}