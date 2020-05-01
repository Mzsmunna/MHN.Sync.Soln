//using GTW.Sync.SQLRepository.DAL;
using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository.Services;
using Ninject.Modules;

namespace MHN.Sync.IOC.Module
{
    public class ConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigService>().To<ConfigService>();
            //Bind<SqlDbContext>().ToSelf().InSingletonScope();
        }
    }
}
