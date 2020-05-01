using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.SQLRepository.Helper
{
    public static class ExtensionMethods
    {
        public static object IsNull<T>(this T Value)
        {
            return string.IsNullOrWhiteSpace(Convert.ToString(Value)) ? (object)DBNull.Value : Value;
        }

        public static SqlParameter ToSqlParameter<T>(this T Value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new Exception("Innvalid Paramter Name.");
            return new SqlParameter($"@{parameterName}", Value.IsNull());
        }
    }
}
