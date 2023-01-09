
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Sanitizer.ApplicationCore.Sanitizers
{
    public class AbcSanitizer : SanitizerBase
    {
        public AbcSanitizer(string prefix, string suffix, IFormFile file) : base(prefix, suffix, file) { }

        public override string sanitizeBlock(string str)
        {
            var regex = @"A[1-9]*C";
            var match = Regex.Match(str, regex, RegexOptions.IgnoreCase);

            return match.Success ? str : "A255C";
        }
    }
}
