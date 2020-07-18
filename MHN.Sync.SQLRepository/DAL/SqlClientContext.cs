using MHN.Sync.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.SQLRepository.DAL
{
    public static class SqlClientContext<T> where T : class
    {      
        public static void QuickBulkInsert(List<T> entityList)
        {
            try
            {
                var datatable = ToDataTable(entityList);
                using (SqlConnection conn = new SqlConnection(ApplicationConstants.DatabaseContext))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = $"{typeof(T).Name}s";
                        bulkCopy.WriteToServer(datatable);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ToDataTable(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
