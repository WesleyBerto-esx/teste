using FunctionApp2.Services;
using functions.Data.Repository;
using functions.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace functions.Functions
{
    public class PostPdfFunction
    {
        public static List<PdfData> dadosArmazenados = new List<PdfData>();
        private readonly ILogger<PostPdfFunction> _logger;
        private readonly PdfService _pdfService;
        private readonly FileService _fileService;
        private readonly ValidationService _validationService;
        private readonly PdfRepository _pdfRepository;

        public PostPdfFunction(
            ILogger<PostPdfFunction> logger,
            PdfRepository pdfRepository,
            PdfService pdfService,
            FileService fileService,
            ValidationService validationService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pdfService = pdfService;
            _fileService = fileService;
            _validationService = validationService;
        }

        [Function("PostPdf")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "pdf/upload")] HttpRequest req)
        {
            _logger.LogInformation("Iniciando o armazenamento de dados.");

            var empresa = req.Form["empresa"].ToString();
            var nota = req.Form["nota"].ToString();
            var pdfFile = req.Form.Files["file"];

            if (string.IsNullOrEmpty(empresa) || string.IsNullOrEmpty(nota) || pdfFile == null)
            {
                return new BadRequestObjectResult("Dados incompletos. Empresa, nota e arquivo são obrigatórios.");
            }

            if (!pdfFile.ContentType.Equals("application/pdf"))
            {
                return new BadRequestObjectResult("O arquivo deve ser um PDF.");
            }

            var storageDirectory = Path.Combine(Path.GetTempPath(), "pdfs");
            var filePath = _fileService.SaveFile(pdfFile.OpenReadStream(), pdfFile.FileName, storageDirectory);
            string pdfContent = _pdfService.ExtractTextFromPdf(filePath);

            var (razaoSocial, numeroNota) = _pdfService.ExtractDataFromPdf(pdfContent);

            //if (!_validationService.ValidatePdfData(pdfContent, "caminho/do/seu/excel.xlsx"))
            //{
            //    return new BadRequestObjectResult("Os dados do PDF não correspondem aos dados do Excel.");
            //}

            string novoNomeArquivo = $"{numeroNota}_{razaoSocial.Replace(" ", "_").ToUpper()}.pdf";
            var novoCaminhoArquivo = _fileService.RenameFile(filePath, novoNomeArquivo);

            var novoRegistro = new PdfData
            {
                Empresa = empresa,
                Nota = nota,
                FilePath = novoCaminhoArquivo,
                Content = pdfContent
            };

             await _pdfRepository.AddPdfDataAsync(novoRegistro);
            _logger.LogInformation($"Empresa: {novoRegistro.Empresa}, Nota: {novoRegistro.Nota}");

            return new OkObjectResult("Dados e arquivo PDF armazenados com sucesso.");
        }

       
    }
}
