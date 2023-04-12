using System.Text;

namespace LibreConvert;

public class LibreOfficeExportOptions
{
    public virtual string FilterName { get; set; }

    public string PageRange { get; set; }

    public string Password { get; set; }

    public string OwnerPassword { get; set; }

    public bool ExportFormFields { get; set; }

    public bool UseLosslessCompression { get; set; }

    public bool UseTaggedPDF { get; set; }

    public int ReduceImageResolution { get; set; }

    public bool ExportNotes { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrEmpty(FilterName))
        {
            sb.Append($"FilterName={FilterName}");
        }

        if (!string.IsNullOrEmpty(PageRange))
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"PageRange={PageRange}");
        }

        if (!string.IsNullOrEmpty(Password))
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"Password={Password}");
        }

        if (!string.IsNullOrEmpty(OwnerPassword))
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"OwnerPassword={OwnerPassword}");
        }

        if (ExportFormFields)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"ExportFormFields=true");
        }

        if (UseLosslessCompression)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"UseLosslessCompression=true");
        }

        if (UseTaggedPDF)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"UseTaggedPDF=true");
        }

        if (ReduceImageResolution > 0)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"ReduceImageResolution={ReduceImageResolution}");
        }

        if (ExportNotes)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }

            sb.Append($"ExportNotes=true");
        }

        return sb.ToString();
    }
}