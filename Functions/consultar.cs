//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;
//using OpenAC.Net.NFSe;

//namespace financeiroFunctions.Functions
//{
//    public class Consultar
//    {
//        private readonly ILogger<Consultar> _logger;

//        public Consultar(ILogger<Consultar> logger)
//        {
//            _logger = logger;
//        }

//        [Function("consultar")]
//        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
//        {
//            try
//            {
//                string cnpj = req.Query["cnpj"];
//                string numeroNfse = req.Query["numeroNfse"];
//                string codigoMunicipio = req.Query["codigoMunicipio"];

//                if (string.IsNullOrEmpty(cnpj) || string.IsNullOrEmpty(numeroNfse) || string.IsNullOrEmpty(codigoMunicipio))
//                {
//                    return new BadRequestObjectResult("CNPJ, número da NFS-e e código do município são obrigatórios.");
//                }

//                var nfse = new Nfse
//                {
//                    CNPJ = cnpj,
//                    CodigoMunicipio = codigoMunicipio
//                };

//                var resultado = await nfse.ConsultarNfseAsync(numeroNfse);

//                if (resultado != null)
//                {
//                    _logger.LogInformation("Status da NFS-e: " + resultado.Status);

//                    return new JsonResult(new { status = resultado.Status, numeroNfse = numeroNfse });
//                }

//                return new NotFoundObjectResult("NFS-e não encontrada.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Erro ao consultar a NFS-e: {ex.Message}");
//                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//            }


//        }

//        public async Task ConsultarNfseAsync(string numeroNfse)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
