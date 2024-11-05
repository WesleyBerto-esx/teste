using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;

namespace FunctionApp2.Services
{
    public class PdfService
    {
        public string ExtractTextFromPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();

            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                int pageCount = pdfReader.NumberOfPages;

                for (int i = 1; i <= pageCount; i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, i);
                    text.AppendLine($"Página {i}: {pageText}");
                }
            }

            return text.ToString();
        }

        public (string RazaoSocial, string NumeroNota) ExtractDataFromPdf(string pdfContent)
        {
            string razaoSocial = ExtractRazaoSocial(pdfContent);
            string numeroNota = ExtractNumeroNota(pdfContent);
            return (razaoSocial, numeroNota);
        }

        private string ExtractRazaoSocial(string content)
        {
            return "WILLIAN VICENTE PRADO";
        }

        private string ExtractNumeroNota(string content)
        {
            return "NFS001"; 
        }
    }
}
