using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository.DAL;

namespace MHN.Sync.MongoRepository.Services
{
    public class ConfigServiceForWizard : IConfigService
    {
        public static string conString { get; set; }
        public static  string dbName { get; set; }
        public ConfigServiceForWizard(string connectionString, string databaseName)
        {
            conString = connectionString;
            dbName = databaseName;
        }

        public static string DatabaseConnectionString
        {
            get
            {
                string valString = null;
                var valSettings = conString;
                if (valSettings != null)
                {
                    valString = valSettings;
                }
                return valString;
            }
        }

        public static string DatabaseName
        {
            get
            {
                string valString = null;
                var valSettings = dbName;
                if (valSettings != null)
                {
                    valString = valSettings;
                }
                return valString;
            }
        }

        public IDatabaseContext DatabaseContext
        {
            get
            {
                return new MongoDbContext();
            }
        }
    }
}
