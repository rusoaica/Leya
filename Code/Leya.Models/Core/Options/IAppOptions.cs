/// Written by: Yulia Danilova
/// Creation Date: 28th of August, 2021
/// Purpose: Interface business model for options

namespace Leya.Models.Core.Options
{
    public interface IAppOptions
    {
        #region ================================================================ PROPERTIES =================================================================================
        IOptionsMedia OptionsMedia { get; }
        IOptionsPlayer OptionsPlayer { get; }
        IOptionsInterface OptionsInterface { get; }
        #endregion
    }
}
