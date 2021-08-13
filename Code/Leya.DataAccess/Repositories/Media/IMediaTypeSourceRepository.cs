/// Written by: Yulia Danilova
/// Creation Date: 02nd of December, 2020
/// Purpose: Media type sources repository interface for the bridge-through between generic storage medium and storage medium for MediaTypeSources
#region ========================================================================= USING =====================================================================================
using Leya.DataAccess.Common.Models.Media;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess.Repositories.Media
{
    public interface IMediaTypeSourceRepository : IRepository<MediaTypeSourceEntity> 
    {
        
    }
}
