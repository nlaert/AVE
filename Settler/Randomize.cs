using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settler
{
    class Randomize
    {
        public const int MIN_RANDOM_INTEGER = 1;
        public const int MAX_RANDOM_INTEGER = 10;
        public const int RANDOM_STRING_SIZE = 10;
        public static string GetRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[RANDOM_STRING_SIZE];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        public static int GetRandomInteger()
        {
            Random random = new Random();
            return random.Next(MIN_RANDOM_INTEGER, MAX_RANDOM_INTEGER);
        }
        public static int GetRandomInteger(int n) 
        {
            Random random = new Random();
            return random.Next(0, n);
        }
    }
}
