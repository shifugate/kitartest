using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ARKit.Util
{
    public class CommonUtil
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }

        public static bool IsPasswordValid(string password)
        {
            return password.Length < 6 || password.Length > 12 ? false: true;
        }

        public static bool IsPasswordRepeatValid(string password, string passwordRepeat)
        {
            return password == passwordRepeat;
        }

        public static bool IsNotEmpty(string name)
        {
            return name != null && name.Trim().Length > 0;
        }

        public static bool IsValidName(string name)
        {
            return name != null && name.Trim().Length > 0 && Regex.IsMatch(name, @"^[a-zA-Z0-9\s]*$");
        }

        public static void ShuffleList<T>(IList<T> list, int seed)
        {
            int index = list.Count;

            Random random = new Random(seed);

            while (index > 1)
            {
                index--;
                int k = random.Next(index + 1);
                T value = list[k];
                list[k] = list[index];
                list[index] = value;
            }
        }
    }
}
