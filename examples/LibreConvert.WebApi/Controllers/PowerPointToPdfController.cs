using LibreConvert.Documents;
using LibreConvert.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibreConvert.WebApi.Controllers;

[ApiController]
[Route("powerPoint-to-pdf")]
public class PowerPointToPdfController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Convert(IFormFile powerPointFile)
    {
        var temporaryInputFilePath = await powerPointFile.SaveToTemporaryFile();
        
        var document = new ImpressDocument(temporaryInputFilePath);

        var generatedPdfFilePath = await document.ExportAsPDF();
        
        var fileContents = await System.IO.File.ReadAllBytesAsync(generatedPdfFilePath);

        System.IO.File.Delete(generatedPdfFilePath);

        return new FileContentResult(fileContents, "application/pdf")
        {
            FileDownloadName = Path.GetFileNameWithoutExtension(powerPointFile.FileName) + $"-{RunningOperatingSystem.Name}-LibreConvert-PowerPoint-to-PDF.pdf"
        };
    }
}