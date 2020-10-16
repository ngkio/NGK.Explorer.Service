using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Explorer.Service.Common
{
    public static class UtilityHelper
    {
        public static bool IsBlockNum(this string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            return Regex.IsMatch(key, "^\\d+$");
        }

        public static bool TryDeserializeObject<T>(string jsonStr, out T result) where T : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonStr)) throw new ArgumentNullException();

                result = JsonConvert.DeserializeObject<T>(jsonStr);
                return true;
            }
            catch (Exception e)
            {
                result = default;
                return false;
            }
        }
    }
}