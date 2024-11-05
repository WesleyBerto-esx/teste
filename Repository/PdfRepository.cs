using functions.Entity;
using functions.Data;


namespace functions.Data.Repository
{
    public class PdfRepository
    {
        private readonly AppDbContext _context;

        public PdfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPdfDataAsync(PdfData pdfData)
        {
            _context.PdfData.Add(pdfData);
            await _context.SaveChangesAsync();
        }
    }
}
