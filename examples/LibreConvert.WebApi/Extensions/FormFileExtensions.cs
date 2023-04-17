namespace LibreConvert.WebApi.Extensions;

public static class FormFileExtensions
{
    public static async Task<string> SaveToTemporaryFile(this IFormFile formFile)
    {
        var tempFilePath = Path.GetTempFileName();

        await using var stream = new FileStream(tempFilePath, FileMode.Create);
        
        await formFile.CopyToAsync(stream);

        return tempFilePath;
    }
}