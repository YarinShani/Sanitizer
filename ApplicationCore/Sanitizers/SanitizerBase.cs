using System.Text;
using System.Text.RegularExpressions;

namespace Sanitizer.ApplicationCore.Sanitizers
{
    public abstract class SanitizerBase
    {
        protected readonly string prefix;
        protected readonly string suffix;
        protected readonly IFormFile file;

        public SanitizerBase(string prefix, string suffix, IFormFile file)
        {  
            this.prefix = prefix;
            this.suffix = suffix;
            this.file = file;   
        }

        public string Sanitize()
        {
            var result = new StringBuilder();
            var block = new StringBuilder();

            using (var stream = new MemoryStream((int)file.Length))
            {
                file.CopyTo(stream);
                byte[] bytes = stream.ToArray();

                int insertions = 0;
                int idx = 0;

                foreach (byte b in bytes)
                {
                    String s = byteToString(b);
                    idx += 1;

                    if (s.Equals("\r") || s.Equals("\n"))
                    {
                        result.Append(s);
                        block.Clear();
                        continue;
                    }


                    block.Append(s);
                    insertions += 1;

                    if (insertions % 3 == 0)
                    {
                        if (insertions == 3)
                        {
                            string blockStr = block.ToString();
                            detectFirstBlock(blockStr);
                            result.Append(blockStr);
                        }
                        else if (idx == file.Length)
                        {
                            string blockStr = block.ToString();
                            detectLastBlock(blockStr);
                            result.Append(blockStr);
                        }
                        else
                        {
                            string blockStr = block.ToString();
                            string sanitizedBlock = sanitizeBlock(blockStr);
                            result.Append(sanitizedBlock);
                        }

                        block.Clear();
                    }
                }
            }

            return result.ToString();
        }

        protected void detectFirstBlock(string str)
        {
            detectBlock(str, "starts", prefix);
        }

        protected void detectLastBlock(string str)
        {
            detectBlock(str, "ends", suffix);
        }

        private void detectBlock(string str, string filePart, string wordPart)
        {
            if (!wordPart.Equals(str))
            {
                throw new Exception($"Wrong file format. The file doesn't {filePart} with {wordPart}");
            }
        }

        public virtual string sanitizeBlock(string str)
        {
            return str;
        }

        public string bytesToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        public string byteToString(byte b)
        {
            return bytesToString(new byte[]{b});
        }
    }
}
