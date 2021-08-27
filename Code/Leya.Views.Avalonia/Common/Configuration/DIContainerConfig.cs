/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Registers services in the Autofac Dependency Injection container
#region ========================================================================= USING =====================================================================================
using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Leya.DataAccess;
using Leya.Infrastructure.Configuration;
using Leya.Infrastructure.Dialog;
using Leya.Infrastructure.Logging;
using Leya.Infrastructure.Notification;
using Leya.Models.Common.Models.Media;
using Leya.Models.Core.Artists;
using Leya.Models.Core.MediaLibrary;
using Leya.Models.Core.Movies;
using Leya.Models.Core.Navigation;
using Leya.Models.Core.Options;
using Leya.Models.Core.Player;
using Leya.Models.Core.Security;
using Leya.Models.Core.TvShows;
using Leya.ViewModels.Common.Clipboard;
using Leya.ViewModels.Common.Dialogs;
using Leya.ViewModels.Common.Dialogs.FileBrowser;
using Leya.ViewModels.Common.Dialogs.FileSave;
using Leya.ViewModels.Common.Dialogs.FolderBrowser;
using Leya.ViewModels.Common.Dialogs.MessageBox;
using Leya.ViewModels.Common.Dispatcher;
using Leya.ViewModels.Common.Models.Media;
using Leya.ViewModels.Common.ViewFactory;
using Leya.ViewModels.Main;
using Leya.ViewModels.Options;
using Leya.ViewModels.Register;
using Leya.ViewModels.Startup;
using Leya.Views.Common.Clipboard;
using Leya.Views.Common.Dialogs;
using Leya.Views.Common.Dialogs.MessageBox;
using Leya.Views.Common.Dispatcher;
using Leya.Views.Common.UIFactory;
using Leya.Views.Main;
using Leya.Views.Options;
using Leya.Views.Register;
using Leya.Views.Startup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
#endregion

namespace Leya.Views.Common.Configuration
{
    public static class DIContainerConfig
    {
        public static IContainer Configure()
        {
            // TODO: use reflection and pattern matching for all repetitive registrations
            ContainerBuilder builder = new ContainerBuilder();


            #region infrastructure
            builder.RegisterType<LoggerManager>().As<ILoggerManager>().SingleInstance();
            builder.RegisterType<LoggerInterceptor>();
            builder.RegisterType<AsyncLoggerInterceptor>();
            #endregion

            #region view models 
            builder.RegisterType<StartupVM>().As<IStartupVM>().InstancePerDependency();
            builder.RegisterType<MsgBoxVM>().As<IMsgBoxVM>().InstancePerDependency();
            builder.RegisterType<RegisterVM>().As<IRegisterVM>().InstancePerDependency();
            builder.RegisterType<RecoverPasswordVM>().As<IRecoverPasswordVM>().InstancePerDependency();
            builder.RegisterType<ChangePasswordVM>().As<IChangePasswordVM>().InstancePerDependency();
            builder.RegisterType<MainWindowVM>().As<IMainWindowVM>().InstancePerDependency();
            //builder.RegisterType<SystemVM>().As<ISystemVM>().InstancePerDependency();
            builder.RegisterType<FolderBrowserDialogVM>().As<IFolderBrowserDialogVM>().InstancePerDependency();
            builder.RegisterType<FileBrowserDialogVM>().As<IFileBrowserDialogVM>().InstancePerDependency();
            builder.RegisterType<FileSaveDialogVM>().As<IFileSaveDialogVM>().InstancePerDependency();
            builder.RegisterType<OptionsMediaVM>().As<IOptionsMediaVM>().InstancePerDependency();
            #endregion

            #region models            

            builder.RegisterType<Authentication>()
                   .As<IAuthentication>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<MediaLibrary>()
                   .As<IMediaLibrary>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<TvShow>()
                   .As<ITvShow>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Season>()
                   .As<ISeason>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Episode>()
                   .As<IEpisode>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Movie>()
                   .As<IMovie>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Artist>()
                   .As<IArtist>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Album>()
                   .As<IAlbum>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<Song>()
                   .As<ISong>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();
            builder.RegisterType<MediaType>()
                   .As<IMediaType>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<MediaTypeSource>()
                   .As<IMediaTypeSource>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<MediaLibraryNavigation>()
                   .As<IMediaLibraryNavigation>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<MediaPlayer>()
                   .As<IMediaPlayer>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<MediaState>()
                   .As<IMediaState>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            builder.RegisterType<OptionsMedia>()
                   .As<IOptionsMedia>()
//#if !DEBUG
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(LoggerInterceptor))
//#endif
                   .SingleInstance();

            #endregion

            #region data access
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

            Type[] dataAccessLayerTypes = Assembly.Load("Leya.DataAccess").GetTypes();
            Type genericRepositoryType = Type.GetType("Leya.DataAccess.Repositories.Common.IRepository`1, Leya.DataAccess");
            Type sqlDataAccessType = Type.GetType("Leya.DataAccess.StorageAccess.SqlDataAccess, Leya.DataAccess");
            Type iDataAccessType = Type.GetType("Leya.DataAccess.StorageAccess.IDataAccess, Leya.DataAccess");
            Type iConnectionStringType = Type.GetType("Leya.DataAccess.StorageAccess.IConnectionString, Leya.DataAccess");
            Type repositoryFactoryType = Type.GetType("Leya.DataAccess.Repositories.Common.RepositoryFactory, Leya.DataAccess");
            Type iRepositoryFactoryType = Type.GetType("Leya.DataAccess.Repositories.Common.IRepositoryFactory, Leya.DataAccess");
            Type sqLiteConnectionType = Type.GetType("System.Data.SQLite.SQLiteConnection, System.Data.SQLite");

            if (File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "appsettings.json"))
                builder.Register(context => JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "appsettings.json")))
                       .OnActivating(e => e.Instance.ConfigurationFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "appsettings.json")
                       .As<IAppConfig>()
                       .SingleInstance();
#if !DEBUG
            else
                throw new FileNotFoundException("Configuration file not found!\nPath: " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)  + Path.DirectorySeparatorChar + "appsettings.json");
#endif
            builder.RegisterType(sqLiteConnectionType).As<IDbConnection>().InstancePerLifetimeScope();


            builder.RegisterType(sqlDataAccessType)
                   .FindConstructorsWith(new InternalConstructorFinder())
                   .As(iDataAccessType)
                   .OnActivating(e => e.Instance.GetType()
                                                .GetProperty("ConnectionString")
                                                .SetValue(e.Instance, e.Context.Resolve<IAppConfig>().ConnectionStrings["SqLite"]))
                   .SingleInstance();
                   //.InstancePerDependency();

            builder.RegisterType(repositoryFactoryType).As(iRepositoryFactoryType).SingleInstance();
            // get all classes implementing IRepository (all repository classes) and register them as their corresponding repository interface
            IEnumerable<Type> repositoryTypes = dataAccessLayerTypes.Where(t => !t.IsInterface &&
                                                                                 t.GetInterfaces()
                                                                                  .Any(i => i.IsGenericType &&
                                                                                            i.GetGenericTypeDefinition() == genericRepositoryType));
            foreach (Type type in repositoryTypes)
            {
                builder.RegisterType(type)
                       .As(type.GetInterfaces()
                               .Where(i => !i.IsGenericType &&
                                            i.GetInterfaces()
                                             .Any(j => j.GetGenericTypeDefinition() == genericRepositoryType))
                               .First())
                       .InstancePerDependency();
            }

            //builder.RegisterType(sqlDataAccessType)
            //       .FindConstructorsWith(new InternalConstructorFinder()).As(iDataAccessType)
            //                                                             .As(iConnectionStringType)
            //                                                             .InstancePerDependency()
            //                                                             .EnableInterfaceInterceptors()
            //                                                             .InterceptedBy(typeof(LoggerInterceptor)); 
#endregion

#region dialogs
            //builder.RegisterType<ClientVM>().As<IClientVM>().InstancePerDependency();
            //builder.RegisterType<MsgBoxVM>().As<IMsgBoxVM>().InstancePerDependency();
            //builder.RegisterType<MockDispatcher>().As<IDispatcher>().InstancePerDependency();
            //builder.RegisterType<WindowsClipboard>().As<IClipboard>().InstancePerDependency();

            //builder.Register(context => new MessageBoxService(context.Resolve<IDispatcher>(), context.Resolve<IMsgBoxVM>())).As<IMessageBoxService>().InstancePerDependency();
            // similar with            
            builder.RegisterType<MessageBoxService>()
                .WithParameter(new ResolvedParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(IDispatcher),
                    (propertyInfo, context) => context.Resolve<IDispatcher>()))
                .As<INotificationService>().InstancePerDependency();

            //builder.RegisterType<MessageBoxService>()
            //   .WithParameter(new ResolvedParameter(
            //       (propertyInfo, context) => propertyInfo.ParameterType == typeof(IDispatcher),
            //       (propertyInfo, context) => context.Resolve<IDispatcher>()))
            //   .WithParameter(
            //       (propertyInfo, context) => propertyInfo.ParameterType == typeof(Func<IMsgBoxVM>),
            //       (propertyInfo, context) =>
            //       {
            //           IComponentContext componentContext = context.Resolve<IComponentContext>();
            //            //return new Func<IMsgBoxVM>(() => componentContext.Resolve<IMsgBoxVM>());
            //            return new Func<IMsgBoxVM>(() => componentContext.Resolve<IMsgBoxVM>());
            //       })
            //   .As<INotificationService>().InstancePerDependency();

            builder.RegisterType<FolderBrowserService>()
                .WithParameter(new ResolvedParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(IDispatcher),
                    (propertyInfo, context) => context.Resolve<IDispatcher>()))
                .WithParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(Func<IFolderBrowserDialogVM>),
                    (propertyInfo, context) =>
                    {
                        IComponentContext componentContext = context.Resolve<IComponentContext>();
                        return new Func<IFolderBrowserDialogVM>(() => componentContext.Resolve<IFolderBrowserDialogVM>());
                    })
                .As<IFolderBrowserService>().InstancePerDependency();

            builder.RegisterType<FileBrowserService>()
                .WithParameter(new ResolvedParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(IDispatcher),
                    (propertyInfo, context) => context.Resolve<IDispatcher>()))
                .WithParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(Func<IFileBrowserDialogVM>),
                    (propertyInfo, context) =>
                    {
                        IComponentContext componentContext = context.Resolve<IComponentContext>();
                        return new Func<IFileBrowserDialogVM>(() => componentContext.Resolve<IFileBrowserDialogVM>());
                    })
                .As<IFileBrowserService>().InstancePerDependency();

            builder.RegisterType<FileSaveService>()
                .WithParameter(new ResolvedParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(IDispatcher),
                    (propertyInfo, context) => context.Resolve<IDispatcher>()))
                .WithParameter(
                    (propertyInfo, context) => propertyInfo.ParameterType == typeof(Func<IFileSaveDialogVM>),
                    (propertyInfo, context) =>
                    {
                        IComponentContext componentContext = context.Resolve<IComponentContext>();
                        return new Func<IFileSaveDialogVM>(() => componentContext.Resolve<IFileSaveDialogVM>());
                    })
                .As<IFileSaveService>().InstancePerDependency();
#endregion

#region views
            //builder.RegisterType<MainWindow>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IMainWindowVM>()).As<IMainWindowView>().InstancePerDependency();
            //builder.RegisterType<Client>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IClientVM>()).As<IClientView>().InstancePerDependency();
            //builder.RegisterType<MsgBoxV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IMsgBoxVM>()).As<IMsgBoxView>().InstancePerDependency();
            // similar with:
            //builder.Register(context => new MainWindow { DataContext = context.Resolve<IMainWindowVM>() }).As<IMainWindowView>().InstancePerDependency();
            //builder.Register(context => new Client { DataContext = context.Resolve<IClientVM>() }).As<IClientView>().InstancePerDependency();
            //builder.Register(context => new MsgBoxV { DataContext = context.Resolve<IMsgBoxVM>() }).As<IMsgBoxView>().InstancePerDependency();

            // views

            builder.RegisterType<StartupV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IStartupVM>()).As<IStartupView>().SingleInstance();
            //builder.RegisterType<SystemV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<ISystemVM>()).As<ISystemView>().InstancePerDependency();
            builder.RegisterType<RegisterV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IRegisterVM>()).As<IRegisterView>().InstancePerDependency();
            builder.RegisterType<MainWindowV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IMainWindowVM>()).As<IMainWindowView>().InstancePerDependency();
            builder.RegisterType<RecoverPasswordV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IRecoverPasswordVM>()).As<IRecoverPasswordView>().InstancePerDependency();
            builder.RegisterType<ChangePasswordV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IChangePasswordVM>()).As<IChangePasswordView>().InstancePerDependency();
            //builder.RegisterType<OptionsMediaV>().OnActivating(e => e.Instance.DataContext = e.Context.Resolve<IOptionsMediaVM>()).As<IOptionsMediaView>().InstancePerDependency();
#endregion


            builder.RegisterType<MsgBoxV>().As<IMsgBoxView>().InstancePerDependency();
            builder.RegisterType<FolderBrowserDialogV>().As<IFolderBrowserDialogView>().InstancePerDependency();
            builder.RegisterType<FileBrowserDialogV>().As<IFileBrowserDialogView>().InstancePerDependency();
            builder.RegisterType<FileSaveDialogV>().As<IFileSaveDialogView>().InstancePerDependency();
            builder.RegisterType<ApplicationDispatcher>().As<IDispatcher>().InstancePerDependency();
            builder.RegisterType<WindowsClipboard>().As<IClipboard>().SingleInstance();

            builder.RegisterType<ViewFactory>().As<IViewFactory>().InstancePerDependency();

            return builder.Build();
        }

        /// <summary>
        /// Adds all the repositories from the Data Access Layer so that they can be exposed to the Business Layer
        /// </summary>
        public static void AddRepositories(IContainer container)
        {
            Type[] dataAccessLayerTypes = Assembly.Load("Leya.DataAccess").GetTypes();
            Type genericRepositoryType = Type.GetType("Leya.DataAccess.Repositories.Common.IRepository`1, Leya.DataAccess");
            Type iRepositoryFactoryType = Type.GetType("Leya.DataAccess.Repositories.Common.IRepositoryFactory, Leya.DataAccess");


            // get all the concrete implementations of IRepository<>
            IEnumerable<Type> repositoryClassTypes =
                dataAccessLayerTypes.Where(t => !t.IsInterface && !t.IsAbstract && t.GetInterfaces()
                                                                                    .Any(i => i.IsGenericType &&
                                                                                              i.GetGenericTypeDefinition() == genericRepositoryType));
            // store all repositories
            foreach (Type repositoryClassType in repositoryClassTypes)
            {
                // get the interface that implements IRepository<> of the currently iterated repository class
                Type repositoryInterfaceType = repositoryClassType
                                            .GetInterfaces()
                                            .Where(i => !i.IsGenericType &&
                                                         i.GetInterfaces()
                                                          .Any(a => a.GetGenericTypeDefinition() == genericRepositoryType))
                                            .First();
                // ask the concrete type for the repository interface type from the repositories factory;
                // because the method for creating a repository is generic and we need to call it with a runtime type, reflection is the only option
                object repositoryClass = iRepositoryFactoryType.GetMethod("CreateRepository")
                                                                   .MakeGenericMethod(repositoryInterfaceType)
                                                                   .Invoke(container.Resolve(iRepositoryFactoryType), null);
                //Repositories.Add(repositoryClass);
            }
        }
    }
}
