using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sanitizer.ApplicationCore.Sanitizers;

namespace Sanitizer.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        
        public FileContentResult OnPostSanitize(IFormFile file)
        {
            String extension = System.IO.Path.GetExtension(file.FileName);
            SanitizerBase s;

            switch (extension)
            {
                case ".abc":
                    s = new AbcSanitizer("123", "789", file);
                    break;
                case ".efg":
                    s = new EfgSanitizer("567", "123", file);
                    break;
                default:
                    throw new Exception("Not supported file");
            }

            string content = s.Sanitize();

            return File(System.Text.Encoding.ASCII.GetBytes(content), file.ContentType, file.FileName);

        }
    }
}