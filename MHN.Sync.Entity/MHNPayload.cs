using Cerebro.JWTAuthService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity
{
    public class MHNPayload : IPayload
    {
        public string unique_name { get; set; }
        public string clientId { get; set; }
        public string company { get; set; }
    }
}
