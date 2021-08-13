/// Written by: Yulia Danilova
/// Creation Date: 23rd of November, 2020
/// Purpose: Data transfer object for the video information

namespace Leya.Models.Common.Models.Movies
{
    public class VideoEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MovieId { get; set; }
        public string Codec { get; set; }
        public float Aspect { get; set; }
        public bool Is3D { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + MovieId;
        }
        #endregion
    }
}
