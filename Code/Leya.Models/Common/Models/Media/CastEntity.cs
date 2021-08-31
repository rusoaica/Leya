/// Written by: Yulia Danilova
/// Creation Date: 29th of August, 2021
/// Purpose: Data transfer object for the list of actors

namespace Leya.Models.Common.Models.Media
{
    public class CastEntity
    {
        #region ================================================================ PROPERTIES =================================================================================
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Thumb { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Customized ToString() method
        /// </summary>
        /// <returns>Custom string value showing relevant data for current class</returns>
        public override string ToString()
        {
            return Id + " :: " + Name;
        }
        #endregion
    }
}
