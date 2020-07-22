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
        public static void CreateTable()
        {
            using (SqlConnection conn = new SqlConnection(ApplicationConstants.DatabaseContext))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string subQuery = string.Empty;

                    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo prop in Props)
                    {
                        if (!prop.Name.ToLower().Equals("id"))
                            subQuery += ",\n\t\t" + prop.Name + "     varchar(200) NULL";
                    }

                    string exactQuery =
                        @" 
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + $"{typeof(T).Name}s" + @"]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE " + $"{ typeof(T).Name}s" + @" (
                          Id     integer PRIMARY KEY NOT NULL"

                                  + subQuery +

                                @"); 
                    END";

                    cmd.CommandText = exactQuery;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

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

        public static void UpdateData(List<T> list)
        {
            //--- INCOMPLETE ------//

            DataTable dt = ToDataTable(list); //new DataTable("MyTable");
            string TableName = $"{typeof(T).Name}s";

            using (SqlConnection conn = new SqlConnection(ApplicationConstants.DatabaseContext))
            {
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    try
                    {
                        conn.Open();

                        //Creating temp table on database
                        command.CommandText = "CREATE TABLE #TmpTable(...)";
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 660;
                            bulkcopy.DestinationTableName = TableName;
                            bulkcopy.WriteToServer(dt);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 300;
                        command.CommandText = "UPDATE T SET ... FROM " + TableName + " T INNER JOIN #TmpTable Temp ON ...; DROP TABLE #TmpTable;";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception properly
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
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
