using System;
using System.Text;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Random format item.
    /// </summary>
    internal class RandomFormatItem : IFormatItem
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random randomizer;

        static RandomFormatItem()
        {
            randomizer = new Random((int)DateTime.Now.ToFileTime());
        }

        /// <inheritdoc />
        public string Name => "Random";

        /// <inheritdoc />
        public string Pattern => "rnd";

        /// <inheritdoc />
        public string GetText()
        {
            var stringBuilder = new StringBuilder(10);

            for (int i = 0; i < 10; i++)
            {
                var randomNumber = randomizer.Next(Chars.Length);
                var randomChar = Chars[randomNumber].ToString();
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }
    }
}