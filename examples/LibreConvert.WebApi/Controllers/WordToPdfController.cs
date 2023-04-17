using LibreConvert.Documents;
using LibreConvert.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibreConvert.WebApi.Controllers;

[ApiController]
[Route("word-to-pdf")]
public class WordToPdfController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Convert(IFormFile wordFile)
    {
        var temporaryInputFilePath = await wordFile.SaveToTemporaryFile();
        
        var document = new WriterDocument(temporaryInputFilePath);

        var generatedPdfFilePath = await document.ExportAsPDF();
        
        var fileContents = await System.IO.File.ReadAllBytesAsync(generatedPdfFilePath);

        System.IO.File.Delete(generatedPdfFilePath);

        return new FileContentResult(fileContents, "application/pdf")
        {
            FileDownloadName = Path.GetFileNameWithoutExtension(wordFile.FileName) + $"-{RunningOperatingSystem.Name}-LibreConvert-Word-to-PDF.pdf"
        };
    }
}