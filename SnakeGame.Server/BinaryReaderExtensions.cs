using System.IO;
using System.Text;

namespace SnakeGame.Server
{
    public static class BinaryReaderExtension
    {
        public static string ReadLine(this BinaryReader reader)
        {
            var result = new StringBuilder();
            bool foundR = false;
            bool foundN = false;
            char ch;
            while ((foundR && foundN) == false)
            {
                try
                {
                    ch = reader.ReadChar();
                }
                catch (EndOfStreamException ex)
                {
                    if (result.Length == 0) return null;
                    else break;
                }

                switch (ch)
                {
                    case '\r':
                        foundR = true;
                        break;
                    case '\n':
                        if (foundR) foundN = true;
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }

            return result.ToString();
        }
    }
}