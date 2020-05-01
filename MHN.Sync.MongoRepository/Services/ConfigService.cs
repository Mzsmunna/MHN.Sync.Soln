using MHN.Sync.Entity;
using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository.DAL;

namespace MHN.Sync.MongoRepository.Services
{
    public class ConfigService: IConfigService
    {
        public static string DatabaseConnectionString
        {
            get
            {
                string valString = null;
                var valSettings = ApplicationConstants.ConnectionString;
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
                var valSettings = ApplicationConstants.DatabaseName;
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
