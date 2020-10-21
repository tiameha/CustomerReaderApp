using System;
using System.Linq;
using System.Text;

namespace CustomerReaderLib.Models
{
    public static class StringExtensions
    {
        // Ie. Input: jimmy  Output: Jimmy
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return "";

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        // Ie. Input: 101 main street  Output: 101 Main Street
        public static string WordsInSentenceFirstCharToUpper(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return "";

            if (!input.Contains(" "))
                return input.FirstCharToUpper();

            StringBuilder sb = new StringBuilder();
            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                sb.Append(words[i]);
                if (i < words.Length - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}
