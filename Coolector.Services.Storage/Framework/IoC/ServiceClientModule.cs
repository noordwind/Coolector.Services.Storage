using System.Linq;
using Autofac;
using Coolector.Common.Security;
using Coolector.Services.Storage.Services;
using Coolector.Services.Storage.Services.Operations;
using Coolector.Services.Storage.Services.Remarks;
using Coolector.Services.Storage.Services.Statistics;
using Coolector.Services.Storage.Services.Users;

namespace Coolector.Services.Storage.Framework.IoC
{
    public class ServiceClientModule : Module
    {
        private readonly static string OperationsSettingsKey = "operations-settings";
        private readonly static string RemarksSettingsKey = "remarks-settings";
        private readonly static string StatisticsSettingsKey = "statistics-settings";
        private readonly static string UsersSettingsKey = "users-settings";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "operations"))
                .Named<ServiceSettings>(OperationsSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "remarks"))
                .Named<ServiceSettings>(RemarksSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "statistics"))
                .Named<ServiceSettings>(StatisticsSettingsKey)
                .SingleInstance();

            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "users"))
                .Named<ServiceSettings>(UsersSettingsKey)
                .SingleInstance();

            builder.RegisterType<ServiceClient>()
                .As<IServiceClient>();

            builder.Register(x => new OperationServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(OperationsSettingsKey)))
                .As<IOperationServiceClient>()
                .SingleInstance();

            builder.Register(x => new RemarkServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(RemarksSettingsKey)))
                .As<IRemarkServiceClient>()
                .SingleInstance();

            builder.Register(x => new StatisticsServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(StatisticsSettingsKey)))
                .As<IStatisticsServiceClient>()
                .SingleInstance();

            builder.Register(x => new UserServiceClient(x.Resolve<IServiceClient>(), 
                x.ResolveNamed<ServiceSettings>(UsersSettingsKey)))
                .As<IUserServiceClient>()
                .SingleInstance();
        }
    }
}