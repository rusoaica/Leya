/// Written by: Yulia Danilova
/// Creation Date: 31st of August, 2021
/// Purpose: Interface business model for media library statistics
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaStatistics
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        string Forecast { get; set; }
        string TotalMediaCountType { get; set; }
        int TotalMediaCount { get; set; }
        int TotalWatchedCount { get; set; }
        int TotalUnwatchedCount { get; set; }
        double Celsius { get; set; }
        double Fahrenheit { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the statistics of <paramref name="mediaType"/> from <paramref name="mediaLibrary"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library from which to get the statistics</param>
        /// <param name="mediaType">The media for which to get the statistics</param>
        void GetMediaTypeStatistics(IMediaLibrary mediaLibrary, MediaTypeEntity mediaType);

        /// <summary>
        /// Gets the weather information from the webpage located at <paramref name="url"/>
        /// </summary>
        /// <param name="url">The weather.com link of the chosen area</param>
        Task GetWeatherInfoAsync(string url);
        #endregion
    }
}
