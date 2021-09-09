/// Written by: Yulia Danilova
/// Creation Date: 18th of June, 2021
/// Purpose: Automapper configuration file, maps data transfer objects between Data Access Layer and Business Logic Layer
#region ========================================================================= USING =====================================================================================
using System;
using AutoMapper;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Leya.Infrastructure.Security;
using Leya.DataAccess.StorageAccess;
using Leya.Models.Common.Extensions;
using Leya.Models.Common.Models.Users;
using Leya.Models.Common.Models.TvShows;
using Leya.Models.Common.Models.Movies;
using Leya.Models.Common.Models.Artists;
#endregion

namespace Leya.Models.Common.Configuration
{
    internal static class AutomapperConfig
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Configures Automapper
        /// </summary>
        internal static IMapper Configure()
        {
            // get the models of the data access layer
            IEnumerable<Type> dataAccessModelTypes = Assembly.Load("Leya.DataAccess")
                                                             .GetTypes()
                                                             .Where(t => !t.IsInterface &&
                                                                          t.GetInterfaces()
                                                                           .Any(i => i == typeof(IStorageEntity)));
            // get the models of the business logic layer
            IEnumerable<Type> domainModelTypes = Assembly.GetExecutingAssembly()
                                                         .GetTypes()
                                                         .Where(t => !t.IsInterface &&
                                                                     !t.Name.Contains("AnonymousType") &&
                                                                      t.Namespace.StartsWith("Leya.Models.Common.Models"));
            MapperConfiguration automapperConfig = new MapperConfiguration(cfg =>
            {
                // iterate all the data access layer models
                foreach (Type dataAccessModelType in dataAccessModelTypes)
                {
                    // check if there is a business logic layer model with the same name as the data access layer model
                    Type domainModelType = domainModelTypes.Where(t => t.Name == dataAccessModelType.Name).FirstOrDefault();
                    if (domainModelType != null)
                    {
                        // create bi-directional mapping, ignoring the properties that do not exist in data access layer models
                        if (domainModelType.Name == nameof(UserEntity)) 
                        {
                            // user models have special treatment, because they contain secure strings, which have special requirements
                            // TODO: remove hard coded references with reflection based ones
                            cfg.CreateMap(dataAccessModelType, typeof(UserEntity), MemberList.Source)
                               .ForMember("Password", opt => opt.MapFrom(src =>  (src as DataAccess.Common.Models.Users.UserEntity).Password.ToSecureString()))
                               .ForMember("SecurityAnswer", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Users.UserEntity).SecurityAnswer.ToSecureString()));                         
                            cfg.CreateMap(typeof(UserEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("Password", opt => opt.MapFrom(src => Uri.EscapeDataString(PasswordHash.Hash(Crypto.Encrypt((src as UserEntity).Password)))))
                               .ForMember("SecurityAnswer", opt => opt.MapFrom(src => Uri.EscapeDataString(PasswordHash.Hash(Crypto.Encrypt((src as UserEntity).SecurityAnswer))))); 
                        }
                        else if (domainModelType.Name == nameof(EpisodeEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(EpisodeEntity), MemberList.Source)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Episodes.EpisodeEntity).IsWatched.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(EpisodeEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsWatched", opt => opt.MapFrom(src =>  (src as EpisodeEntity).IsWatched.ToString()));
                        }
                        else if (domainModelType.Name == nameof(SeasonEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(SeasonEntity), MemberList.Source)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Seasons.SeasonEntity).IsWatched.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(SeasonEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as SeasonEntity).IsWatched.ToString()));
                        }
                        else if (domainModelType.Name == nameof(TvShowEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(TvShowEntity), MemberList.Source)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.TvShows.TvShowEntity).IsWatched.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(TvShowEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as TvShowEntity).IsWatched.ToString()));
                        }
                        else if (domainModelType.Name == nameof(SongEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(SongEntity), MemberList.Source)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Songs.SongEntity).IsListened.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(SongEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as SongEntity).IsListened.ToString()));
                        }
                        else if (domainModelType.Name == nameof(AlbumEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(AlbumEntity), MemberList.Source)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Albums.AlbumEntity).IsListened.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(AlbumEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as AlbumEntity).IsListened.ToString()));
                        }
                        else if (domainModelType.Name == nameof(ArtistEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(ArtistEntity), MemberList.Source)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Artists.ArtistEntity).IsListened.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(ArtistEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsListened", opt => opt.MapFrom(src => (src as ArtistEntity).IsListened.ToString()));
                        }
                        else if (domainModelType.Name == nameof(MovieEntity))
                        {
                            cfg.CreateMap(dataAccessModelType, typeof(MovieEntity), MemberList.Source)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as DataAccess.Common.Models.Movies.MovieEntity).IsWatched.GetValueOrNull<bool>()));
                            cfg.CreateMap(typeof(MovieEntity), dataAccessModelType, MemberList.Destination)
                               .ForMember("IsWatched", opt => opt.MapFrom(src => (src as MovieEntity).IsWatched.ToString()));
                        }
                        else
                        {
                            cfg.CreateMap(dataAccessModelType, domainModelType, MemberList.Source);
                            cfg.CreateMap(domainModelType, dataAccessModelType, MemberList.Destination);
                        }
                    }
                }
            });
            // make sure all the mappings are valid
            try
            {
                automapperConfig.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                throw;
            }
            return new Mapper(automapperConfig);
        }
        #endregion
    }
}
