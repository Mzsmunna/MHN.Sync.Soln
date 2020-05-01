using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerebro.JWTAuthService.Services.Models
{
    public class Field
    {
        public string Key { get; set; }
        public object Values { get; set; }
    }

    public class ExtendedField : Field
    {
        public string CustomAttribute { get; set; }
    }
}
