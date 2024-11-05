using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace functions.Entity
{
    [Table("PdfData")]
    public class PdfData
    {
        [Key]
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string Nota { get; set; }
        public string FilePath { get; set; }

        public DateTimeOffset DataCriacao { get; set; }

        public string Content { get; set; }
    }
}
