using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Web.Files
{
    public static class FileSaver
    {
        private readonly static IWebHostEnvironment _env = WebApplication.CreateBuilder().Environment;

        public static async Task<string> SaveConfirmationAsync(byte[] pdfBytes, string filename) 
        {
            var confirmationsPath = Path.Combine(_env.ContentRootPath, "wwwroot/confirmations");
            if (!Directory.Exists(confirmationsPath))
            {
                Directory.CreateDirectory(confirmationsPath);
            }

            var fullpath = Path.Combine(confirmationsPath, filename);
            await File.WriteAllBytesAsync(fullpath, pdfBytes);

            return $"/wwwroot/confirmations/{filename}";
        }

        public static async Task<string> SaveInvoiceAsync(byte[] invoiceBytes, string filename) 
        {
            var invoicesPath = Path.Combine(_env.ContentRootPath, "wwwroot/invoices");
            if (!Directory.Exists(invoicesPath))
            {
                Directory.CreateDirectory(invoicesPath);
            }

            var fullpath = Path.Combine(invoicesPath, filename);
            await File.WriteAllBytesAsync(fullpath, invoiceBytes);

            return $"/wwwroot/invoices/{filename}";
        }
    }
}
