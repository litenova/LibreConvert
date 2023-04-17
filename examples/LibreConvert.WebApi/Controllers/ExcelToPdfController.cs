using LibreConvert.Documents;
using LibreConvert.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibreConvert.WebApi.Controllers;

[ApiController]
[Route("excel-to-pdf")]
public class ExcelToPdfController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Convert(IFormFile excelFile)
    {
        var temporaryInputFilePath = await excelFile.SaveToTemporaryFile();
        
        var document = new CalcDocument(temporaryInputFilePath);

        var generatedPdfFilePath = await document.ExportAsPDF();
        
        var fileContents = await System.IO.File.ReadAllBytesAsync(generatedPdfFilePath);

        System.IO.File.Delete(generatedPdfFilePath);

        return new FileContentResult(fileContents, "application/pdf")
        {
            FileDownloadName = Path.GetFileNameWithoutExtension(excelFile.FileName) + $"-{RunningOperatingSystem.Name}-LibreConvert-Excel-to-PDF.pdf"
        };
    }
}