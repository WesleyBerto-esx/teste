using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace functions.Entity
{
    public class PdfData
    {
        public string MunicipioNFS { get; set; }
        public string Prefeitura { get; set; }
        public string NumeroNFS { get; set; }
        public string ChaveAcesso { get; set; }
        public string DataEmissao { get; set; }
        public string DataCompetencia { get; set; }
        public string SerieDPS { get; set; }
        public string EmitenteNome { get; set; }
        public string EmitenteCNPJ { get; set; }
        public string ValorServico { get; set; }
        public string ValorDesconto { get; set; }


    }
}
