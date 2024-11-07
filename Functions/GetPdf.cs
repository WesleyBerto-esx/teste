//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;

//namespace functions.Functions
//{
//    public class GetPdfFunction
//    {
//        private readonly ILogger<GetPdfFunction> _logger;

//        public GetPdfFunction(ILogger<GetPdfFunction> logger)
//        {
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        [Function("GetPdf")]
//        public IActionResult Run(
//            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pdf/{nota}")] HttpRequest req,
//            string nota)
//        {
//            //var pdfData = PostPdfFunction.dadosArmazenados.Find(d => d.Nota == nota);

//            if (pdfData == null || !File.Exists(pdfData.FilePath))
//            {
//                return new NotFoundObjectResult("Arquivo PDF não encontrado.");
//            }

//            byte[] fileBytes = File.ReadAllBytes(pdfData.FilePath);

//            return new FileContentResult(fileBytes, "application/pdf")
//            {
//                FileDownloadName = Path.GetFileName(pdfData.FilePath)
//            };
//        }
//    }
// }
