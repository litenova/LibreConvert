using FluentAssertions;
using LibreConvert.Documents;
using LibreConvert.UnitTests.Samples;

namespace LibreConvert.UnitTests.WriterDocuments;

public class WriterDocumentToPdfConversionTests
{
    [Fact]
    public async Task converting_writer_document_to_PDF_should_create_a_non_empty_pdf()
    {
        // arrange
        var document = new WriterDocument(SampleCatalog.Word.OnlyText1pg);

        // act
        var generatedPdfFullPath = await document.ExportAsPDF();
        
        // assert
        var generatedPdf = new FileInfo(generatedPdfFullPath);
        generatedPdf.Exists.Should().BeTrue();
        generatedPdf.Length.Should().BeGreaterThan(0);
    }
}