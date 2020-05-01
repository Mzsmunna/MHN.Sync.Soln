using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository.Services;
using Ninject.Modules;

namespace MHN.Sync.IOC.Module
{
    public class ConfigViewerModule : NinjectModule
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public ConfigViewerModule(string connectionString, string dbName)
        {
            ConnectionString = connectionString;
            DatabaseName = dbName;
        }
        public override void Load()
        {
            Bind<IConfigService>().To<ConfigServiceForWizard>()
                .WithConstructorArgument("connectionString", ConnectionString)
                .WithConstructorArgument("databaseName", DatabaseName);
        }
    }
}

