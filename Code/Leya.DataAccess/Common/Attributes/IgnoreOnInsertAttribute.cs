/// Written by: Yulia Danilova
/// Creation Date: 18th of May, 2021
/// Purpose: Custom attribute for DTO properties that should be ignored in the process of inserts
#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Leya.DataAccess.Common.Attributes
{
    internal class IgnoreOnInsertAttribute : Attribute
    {

    }
}
