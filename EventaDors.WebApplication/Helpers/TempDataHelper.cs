using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EventaDors.WebApplication.Helpers
{
    public static class TempDataHelper
    {
        public static ITempDataDictionary TempDataDictionary { get; set; }

        public static string Get(string key)
        {
            if(TempDataDictionary.ContainsKey(key))
                return (string) TempDataDictionary[key];
            return null;
        }

        public static void Remove(string key)
        {
            if (TempDataDictionary.ContainsKey(key))
            {
                TempDataDictionary.Remove(key);
            }
        }

        public static void Set(string key, string value)
        {
            if (TempDataDictionary.ContainsKey(key))
                TempDataDictionary[key] = value;
            else
            {
                TempDataDictionary.Add(key, value);
            }
        }
    }
}