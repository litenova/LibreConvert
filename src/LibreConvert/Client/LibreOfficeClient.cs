using uno;
using uno.util;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.bridge;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.uno;
using unoidl.com.sun.star.util;

namespace LibreConvert.Client;

public class LibreOfficeClient : ILibreOfficeClient
{
    private readonly XComponentLoader _componentLoader;

    public LibreOfficeClient(string connectionString, string executablePath)
    {
        var localContext = Bootstrap.defaultBootstrap_InitialComponentContext(null, null);
        var localServiceManager = localContext.getServiceManager();
        var urlResolver = (XUnoUrlResolver) localServiceManager.createInstanceWithContext("com.sun.star.bridge.UnoUrlResolver", localContext);
        XComponentContext remoteContext;

        var i = 0;

        while (true)
        {
            try
            {
                remoteContext = (XComponentContext) urlResolver.resolve($"uno:{connectionString}");
                break;
            }
            catch (System.Exception exception)
            {
                if (i == 20 || !exception.Message.Contains("couldn't connect to pipe")) throw;
                Thread.Sleep(100);
                i++;
            }
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        var remoteFactory = (XMultiServiceFactory) remoteContext.getServiceManager();
        _componentLoader = (XComponentLoader) remoteFactory.createInstance("com.sun.star.frame.Desktop");
    }

    private static string StandardizeFilePath(string filePath)
    {
        return $"file:///{filePath.Replace(@"\", "/")}";
    }

    public void Convert(string inputFile, string outputFile)
    {
        var component = LoadDocumentAsComponent(StandardizeFilePath(inputFile), "_blank");

        ExportToPdf(component, inputFile, outputFile);

        var closeable = (XCloseable) component;

        closeable.close(true);
    }

    /// <summary>
    ///     Creates a new document in LibreOffice and opens the given <paramref name="inputFile" />
    /// </summary>
    /// <param name="aLoader"></param>
    /// <param name="inputFile"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private XComponent LoadDocumentAsComponent(string inputFile, string target)
    {
        var openProps = new PropertyValue[2];
        openProps[0] = new PropertyValue {Name = "Hidden", Value = new Any(true)};
        openProps[1] = new PropertyValue {Name = "ReadOnly", Value = new Any(true)};

        var xComponent = _componentLoader.loadComponentFromURL(inputFile, target, 0, openProps);

        return xComponent;
    }

    /// <summary>
    ///     Exports the loaded document to PDF format
    /// </summary>
    /// <param name="component"></param>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    private static void ExportToPdf(XComponent component, string inputFile, string outputFile)
    {
        var propertyValues = new PropertyValue[3];
        var filterData = new PropertyValue[5];

        filterData[0] = new PropertyValue
        {
            Name = "UseLosslessCompression",
            Value = new Any(false)
        };

        filterData[1] = new PropertyValue
        {
            Name = "Quality",
            Value = new Any(90)
        };

        filterData[2] = new PropertyValue
        {
            Name = "ReduceImageResolution",
            Value = new Any(true)
        };

        filterData[3] = new PropertyValue
        {
            Name = "MaxImageResolution",
            Value = new Any(300)
        };

        filterData[4] = new PropertyValue
        {
            Name = "ExportBookmarks",
            Value = new Any(false)
        };

        // Setting the filter name
        propertyValues[0] = new PropertyValue
        {
            Name = "FilterName",
            Value = new Any(GetFilterType(inputFile))
        };

        // Setting the flag for overwriting
        propertyValues[1] = new PropertyValue {Name = "Overwrite", Value = new Any(true)};

        var polymorphicType = PolymorphicType.GetType(typeof(PropertyValue[]), "unoidl.com.sun.star.beans.PropertyValue[]");

        propertyValues[2] = new PropertyValue {Name = "FilterData", Value = new Any(polymorphicType, filterData)};

        // ReSharper disable once SuspiciousTypeConversion.Global
        ((XStorable) component).storeToURL(StandardizeFilePath(outputFile), propertyValues);
    }

    /// <summary>
    ///     Returns the filter that is needed to convert the given <paramref name="fileName" />,
    ///     <c>null</c> is returned when the file cannot be converted
    /// </summary>
    /// <param name="fileName">The file to check</param>
    /// <returns></returns>
    private static string GetFilterType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        extension = extension?.ToUpperInvariant();

        switch (extension)
        {
            case ".DOC":
            case ".DOT":
            case ".DOTX":
            case ".DOCM":
            case ".DOCX":
            case ".DOTM":
            case ".ODT":
            case ".RTF":
            case ".MHT":
            case ".WPS":
            case ".WRI": return "writer_pdf_Export";

            case ".XLS":
            case ".XLT":
            case ".XLW":
            case ".XLSB":
            case ".XLSM":
            case ".XLSX":
            case ".XLTM":
            case ".XLTX": return "calc_pdf_Export";

            case ".POT":
            case ".PPT":
            case ".PPS":
            case ".POTM":
            case ".POTX":
            case ".PPSM":
            case ".PPSX":
            case ".PPTM":
            case ".PPTX":
            case ".ODP": return "impress_pdf_Export";

            default: throw new NotSupportedException();
        }
    }
}