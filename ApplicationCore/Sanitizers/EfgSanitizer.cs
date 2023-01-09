using System.Text;
using System.Text.RegularExpressions;

namespace Sanitizer.ApplicationCore.Sanitizers
{
    public class EfgSanitizer: SanitizerBase
    {
        public EfgSanitizer(string prefix, string suffix, IFormFile file) : base(prefix, suffix, file) { }

        public override string sanitizeBlock(string str)
        {
            var regex = @"E[a-z]*G";
            var match = Regex.Match(str, regex, RegexOptions.None);

            return match.Success ? str : "E255G";
        }
    }
}
