using functions.Entity;

namespace financeiroFunctions.Entidades
{
    public interface IExtractFromPdf
    {
        string ExtractPdfDataAsJson(Stream pdfStream);

    }
}