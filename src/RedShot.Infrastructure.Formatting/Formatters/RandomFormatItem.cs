using System;
using System.Text;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Random format item.
    /// </summary>
    internal class RandomFormatItem : IFormatItem
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
            var stringBuilder = new StringBuilder(15);

            for (int i = 0; i < 15; i++)
            {
                var randomNumber = randomizer.Next(chars.Length);
                var randomChar = chars[randomNumber].ToString();
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }
    }
}