/// Written by: Yulia Danilova
/// Creation Date: 18th of June, 2021
/// Purpose: Class for services used by Business Logic Layer
#region ========================================================================= USING =====================================================================================
using AutoMapper;
using Leya.Models.Common.Configuration;
#endregion

namespace Leya.Models.Common.Infrastructure
{
    internal static class Services
    {
        #region ================================================================ PROPERTIES =================================================================================
        internal static IMapper AutoMapper { get; } = AutomapperConfig.Configure();
        #endregion
    }
}
