using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.IO;

namespace YourNamespace.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;

        public OpenAiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ConvertPdfToJsonAsync(byte[] pdfData)
        {
            string pdfText;

            using (var memoryStream = new MemoryStream(pdfData))
            {
                using (PdfReader pdfReader = new PdfReader(memoryStream))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    StringBuilder textBuilder = new StringBuilder();
                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        var strategy = new LocationTextExtractionStrategy();
                        string pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
                        textBuilder.Append(pageText);
                    }
                    pdfText = textBuilder.ToString();
                }
            }

            var jsonData = JsonConvert.SerializeObject(new
            {
                message = pdfText,
                instruction = "Por favor, extraia os seguintes campos do texto e ignore todos os outros campos: \r\nNome Empresarial, Número da Nota, CNAE e Valor do Serviço. Apresente esses campos no formato:\r\nNome Empresarial: [conteúdo], Número da Nota: [conteúdo], CNAE: [conteúdo], Valor do Serviço: [conteúdo]."
            });

            Console.WriteLine("JSON gerado a partir do PDF:");
            //Console.WriteLine(jsonData);

            return jsonData;
        }

        public async Task<HttpResponseMessage> SendDataToOpenAi(string jsonData)
        {
            try
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync("https://esxsitefunctions.azurewebsites.net/api/openai", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"Erro ao enviar dados para a API: {errorContent}");
                    return response;
                }

                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de solicitação HTTP: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro desconhecido: {ex.Message}");
                throw;
            }
        }
    }
}
