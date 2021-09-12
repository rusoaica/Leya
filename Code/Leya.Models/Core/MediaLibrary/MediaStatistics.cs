/// Written by: Yulia Danilova
/// Creation Date: 31st of August, 2021
/// Purpose: Business model for media library statistics
#region ========================================================================= USING =====================================================================================
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Leya.Models.Common.Broadcasting;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaStatistics : NotifyPropertyChanged, IMediaStatistics
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly HttpClient weatherScrapper = new HttpClient();
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private string forecast;
        public string Forecast
        {
            get { return forecast; }
            set { forecast = value; Notify(); }
        }

        private string totalMediaCountType = "TV SHOWS:";
        public string TotalMediaCountType
        {
            get { return totalMediaCountType; }
            set { totalMediaCountType = value; Notify(); }
        }

        private int totalMediaCount;
        public int TotalMediaCount
        {
            get { return totalMediaCount; }
            set { totalMediaCount = value; Notify(); }
        }

        private int totalWatchedCount;
        public int TotalWatchedCount
        {
            get { return totalWatchedCount; }
            set { totalWatchedCount = value; Notify(); }
        }

        private int totalUnwatchedCount;
        public int TotalUnwatchedCount
        {
            get { return totalUnwatchedCount; }
            set { totalUnwatchedCount = value; Notify(); }
        }

        private double celsius;
        public double Celsius
        {
            get { return celsius; }
            set { celsius = value; Notify(); }
        }

        private double fahrenheit;
        public double Fahrenheit
        {
            get { return fahrenheit; }
            set { fahrenheit = value; Notify(); }
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the statistics of <paramref name="mediaType"/> from <paramref name="mediaLibrary"/>
        /// </summary>
        /// <param name="mediaLibrary">The media library from which to get the statistics</param>
        /// <param name="mediaType">The media for which to get the statistics</param>
        public void GetMediaTypeStatistics(IMediaLibrary mediaLibrary, MediaTypeEntity mediaType)
        {
            // only get statistics if the media type has any
            if (mediaType != null && mediaType.IsMedia)
            {
                switch (mediaType.MediaType)
                {
                    case "TV SHOW":
                        TotalMediaCountType = "TV SHOWS:";
                        TotalMediaCount = mediaLibrary.Library.TvShows?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.TvShows?.Where(t => t.IsWatched == true)
                                                                         .Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.TvShows?.Where(t => !t.IsWatched == true)
                                                                           .Count() ?? 0;
                        break;
                    case "MOVIE":
                        TotalMediaCountType = "MOVIES:";
                        TotalMediaCount = mediaLibrary.Library.Movies?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.Movies?.Where(m => m.IsWatched == true)
                                                                        .Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.Movies?.Where(m => !m.IsWatched == true)
                                                                          .Count() ?? 0;
                        break;
                    case "MUSIC":
                        TotalMediaCountType = "ARTISTS:";
                        TotalMediaCount = mediaLibrary.Library.Artists?.Count() ?? 0;
                        TotalWatchedCount = mediaLibrary.Library.Artists?.Where(a => a.IsListened == true)
                                                                         .Count() ?? 0;
                        TotalUnwatchedCount = mediaLibrary.Library.Artists?.Where(a => !a.IsListened == true)
                                                                           .Count() ?? 0;
                        break;
                }
            }
            else
            {
                // get defaults
                TotalMediaCountType = "ALL MEDIA:";
                TotalMediaCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?
                                                               .SelectMany(s => s.Episodes)?
                                                               .Count() ?? 0 + 
                                  mediaLibrary.Library.Movies?.Count() ?? 0 + 
                                  mediaLibrary.Library.Artists?.SelectMany(t => t.Albums)?
                                                               .SelectMany(s => s.Songs)?
                                                               .Count() ?? 0;
                TotalWatchedCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?
                                                                 .SelectMany(s => s.Episodes)?
                                                                 .Where(e => e.IsWatched == true)?
                                                                 .Count() ?? 0 + 
                                    mediaLibrary.Library.Movies?.Where(m => m.IsWatched == true)?
                                                                .Count() ?? 0;
                TotalUnwatchedCount = mediaLibrary.Library.TvShows?.SelectMany(t => t.Seasons)?
                                                                   .SelectMany(s => s.Episodes)?
                                                                   .Where(e => !e.IsWatched == true)?
                                                                   .Count() ?? 0 + 
                                      mediaLibrary.Library.Movies?.Where(m => !m.IsWatched == true)?
                                                                  .Count() ?? 0;
            }
        }

        /// <summary>
        /// Gets the weather information from the webpage located at <paramref name="url"/>
        /// </summary>
        /// <param name="url">The weather.com link of the chosen area</param>
        public async Task GetWeatherInfoAsync(string url)
        {
            // it is important to set an agent for the request, otherwise the server weather.com will reject the connection
            weatherScrapper.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            weatherScrapper.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)");
            // scrap the data from weather.com and get the temperature from the returned string
            string temperature = await weatherScrapper.GetStringAsync(url);
            temperature = temperature.Substring(temperature.IndexOf("<div class=\"CurrentConditions--primary--") + 40);
            temperature = temperature.Substring(temperature.IndexOf("<span") + 5);
            temperature = temperature.Substring(temperature.IndexOf(">") + 1);
            // get the weather condition from the returned string
            string _condition = temperature.Substring(temperature.IndexOf("<div data-") + 10);
            temperature = temperature.Substring(0, temperature.IndexOf("°</span>"));
            _condition = _condition.Substring(_condition.IndexOf(">") + 1);
            _condition = _condition.Substring(0, _condition.IndexOf("</div>"));
            // update temperatures and forecast
            Fahrenheit = double.Parse(temperature);
            Celsius = (Fahrenheit - 32) / 1.8;
            Forecast = _condition;
        }
        #endregion
    }
}
