using Cerebro.JWTAuthService.Services.Helper;
using MHN.Sync.Entity;
using MHN.Sync.Entity.Enum;
using MHN.Sync.MongoInterface;
using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository;
using MHN.Sync.SQLInterface;
using MHN.Sync.SQLRepository;
using MHN.Sync.SQLRepository.DAL;
using Ninject;
using Ninject.Modules;

namespace MHN.Sync.IOC.Module
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            // Get config service
            var configService = Kernel.Get<IConfigService>();

            // Bind repositories
            Bind<IProspectMemberRepository>().To<ProspectMemberRepository>().WithConstructorArgument("dbContext", configService.DatabaseContext);
            Bind<IProspectRepository>().To<ProspectRepository>().WithConstructorArgument("dbContext", configService.DatabaseContext);
            Bind<IMemberRepository>().To<MemberRepository>().WithConstructorArgument("dbContext", configService.DatabaseContext);
            Bind<IMemberAdditionalRepository>().To<MemberAdditionalRepository>().WithConstructorArgument("dbContext", configService.DatabaseContext);

            Bind(typeof(IRepository<>)).To(typeof(Repository<>));


            Bind<IUnitOfWork>().To<UnitOfWork>();
            var _secretKey = ApplicationConstants.Get<string>(ConstantType.SCDNSecret);
            var _payload = new MHNPayload
            {
                company = ApplicationConstants.Get<string>(ConstantType.CompanyPayload),
                clientId = ApplicationConstants.ClientId,
                unique_name = ApplicationConstants.Get<string>(ConstantType.UniqueNamePayload)
            };

            Bind<JWTHelperUtility>().ToSelf().InSingletonScope().WithConstructorArgument("secretKey", _secretKey).WithConstructorArgument("payload", _payload);

        }
    }
}
