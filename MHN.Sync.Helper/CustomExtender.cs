using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Helper
{
    public static class CustomExtender
    {
        public static StringBuilder CustomAppender(this StringBuilder str, string appendableString)
        {
            Console.WriteLine(appendableString);
            return str.AppendLine(appendableString);
        }
    }
}
