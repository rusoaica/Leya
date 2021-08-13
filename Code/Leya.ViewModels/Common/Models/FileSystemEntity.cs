﻿/// Written by: Yulia Danilova
/// Creation Date: 28th of June, 2021
/// Purpose: Model for files and directories displayed in custom folder/file browser dialogs

namespace Leya.ViewModels.Common.Models
{
    public class FileSystemEntity 
    {
        #region =============================================================== PROPERTIES ==================================================================================
        public int DirType { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string IconSource { get; set; }
        #endregion
    }
}
