using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Invoiceasy.Helper
{
    public static class CommonHelper
    {
        public static string GenerateID()
        {
            string id = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            return id;
        }

        public static List<T> CloneList<T>(List<T> oldList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (List<T>)formatter.Deserialize(stream);
        }
    }
}
