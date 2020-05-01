using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Entity.MongoEntity
{
    public class Address : IEntity
    {
        public ResidentialAddress residentialAddress { get; set; }
        public MailingAddress mailingAddress { get; set; }

        //public string address { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public string zip { get; set; }
    }

    public class ResidentialAddress : IEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
    public class MailingAddress : IEntity
    {

    }
}
