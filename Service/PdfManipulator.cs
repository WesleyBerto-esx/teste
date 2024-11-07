using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using financeiroFunctions.Entidades;
using functions.Entity;

namespace FunctionApp2.Services
{
    public class ExtractFromPdf : IExtractFromPdf
    {
        public string ExtractPdfDataAsJson(Stream pdfStream)
        {
            var nfseData = new PdfData();
            string pdfText = ExtractTextFromPdf(pdfStream);
            Console.WriteLine(pdfText);

            nfseData.MunicipioNFS = ExtractField(pdfText, @"MUNICÍPIO\s+DE\s+([^\r\n]+)") ?? ExtractField(pdfText, @"Prefeitura\s+Municipal\s+de\s+([^\r\n]+)");
            nfseData.NumeroNFS = ExtractField(pdfText, @"(?:N[uú]mero da|Nota Mestre|Nº NFS-e)\s*[:\-]?\s*(\d+)");
            nfseData.ChaveAcesso = ExtractField(pdfText, @"Chave\s+de\s+Acesso\s*(\d+)");
            nfseData.DataEmissao = ExtractField(pdfText, @"(?:Data e Hora da|Data de) emiss[aã]o\s*([\d/]+ \d{2}:\d{2}:\d{2})") ?? ExtractField(pdfText, @"(?:Data da Emissão|Data e Hora da emiss[aã]o da NFS-e)\s*([\d/]+)");
            nfseData.DataCompetencia = ExtractField(pdfText, @"Compet[eê]ncia da NFS-e\s*([\d/]+)");
            nfseData.SerieDPS = ExtractField(pdfText, @"S[eé]rie da DPS\s*(\d+)");
            nfseData.EmitenteNome = ExtractField(pdfText, @"(?:Raz[aã]o Social|Nome Empresarial|Nome/Razão Social|Emissor:)\s*[:\-]?\s*([A-Za-zÀ-ÿ\s\-]+)");
            nfseData.EmitenteCNPJ = ExtractField(pdfText, @"\s(\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2})");
            nfseData.ValorServico = ExtractField(pdfText, @"(?:VALOR TOTAL DA NOTA|Valor Bruto da Nota|Valor do Servi[cç]o):\s*R?\$\s*([\d,\.]+)");
            nfseData.ValorDesconto = ExtractField(pdfText, @"Desconto\s*Condicionado\s*R\$\s*([\d,\.]+)");

            return JsonConvert.SerializeObject(nfseData, Formatting.Indented);
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            string result = "";
            using (var pdfReader = new PdfReader(pdfStream))
            using (var pdfDoc = new PdfDocument(pdfReader))
            {
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    string content = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                    result += content;
                }
            }
            return result;
        }

        private string ExtractField(string text, string pattern)
        {
            var match = Regex.Match(text, pattern);
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
