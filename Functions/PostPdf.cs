using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using YourNamespace.Services;
using System.Threading.Tasks;
using FunctionApp2.Services;
using financeiroFunctions.Entidades;
using functions.Entity;
using Newtonsoft.Json;
using System.IO;

public class PostPdfFunction
{
    private readonly IExtractFromPdf _extractFromPdf;
    private readonly OpenAiService _openAiService;
    private readonly ILogger<PostPdfFunction> _logger;

    public PostPdfFunction(IExtractFromPdf extractFromPdf, OpenAiService openAiService, ILogger<PostPdfFunction> logger)
    {
        _extractFromPdf = extractFromPdf;
        _openAiService = openAiService;
        _logger = logger;
    }

    [Function("PostPdf")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
    {
        HttpResponseData response;

        try
        {
            byte[] pdfData;
            using (var memoryStream = new MemoryStream())
            {
                await req.Body.CopyToAsync(memoryStream);
                pdfData = memoryStream.ToArray();
            }

            string jsonData = _extractFromPdf.ExtractPdfDataAsJson(new MemoryStream(pdfData));

            if (string.IsNullOrEmpty(jsonData))
            {
                _logger.LogError("Falha ao converter o PDF para JSON.");
                response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Erro: Não foi possível extrair dados do PDF.");
                return response;
            }

            PdfData pdfDataObj = JsonConvert.DeserializeObject<PdfData>(jsonData);

            _logger.LogInformation("Razao Social: " + pdfDataObj.EmitenteNome);
            _logger.LogInformation("CNPJ: " + pdfDataObj.EmitenteCNPJ);
            _logger.LogInformation("Número da Nota: " + pdfDataObj.NumeroNFS);
            _logger.LogInformation("CNAE: " + pdfDataObj.SerieDPS);
            _logger.LogInformation("Valor: " + pdfDataObj.ValorServico);
            _logger.LogInformation("Prefeitura: " + pdfDataObj.Prefeitura);
            _logger.LogInformation("Município: " + pdfDataObj.MunicipioNFS);

      
            response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(jsonData);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na função PostPdf: {ex.Message}");
            response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await response.WriteStringAsync("Erro interno ao processar o PDF.");
        }

        return response;
    }
}
