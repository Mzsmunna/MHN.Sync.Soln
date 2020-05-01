using System.Collections.Generic;
using System.Linq;

namespace Cerebro.JWTAuthService.Services.Models
{
    public class EmailList
    {
        public string TO { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public List<Field> OptionalContent { get; set; }
    }

    public static class Helper
    {
        public static string GetValue(this List<Field> keyValuePairs, string key)
        {
            if(keyValuePairs != null)
            {
                return (string)keyValuePairs.FirstOrDefault(x => x.Key.Equals(key)).Values;
            }
            return string.Empty;
        }
    }
}
