/// Written by: Yulia Danilova
/// Creation Date: 25th of July, 2021
/// Purpose: Class for managing the application's styles
#region ========================================================================= USING =====================================================================================
using System;
using Avalonia;
using Leya.Infrastructure.Enums;
using Avalonia.Markup.Xaml.Styling;
#endregion

namespace Leya.Views.Common.Styles
{
    public sealed class StyleManager
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly Application application;
        private readonly StyleInclude darkStyle = CreateStyle("avares://Leya.ViewsAvalonia/Common/Styles/Dark.xaml");
        private readonly StyleInclude lightStyle = CreateStyle("avares://Leya.ViewsAvalonia/Common/Styles/Light.xaml");
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public Themes CurrentTheme { get; private set; } = Themes.Dark;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="application">The application for which to manage the style</param>
        public StyleManager(Application application)
        {
            this.application = application;
            // safe guard
            if (application.Styles.Count == 0)
                application.Styles.Add(darkStyle);
            else 
                application.Styles[1] = darkStyle;
        }
        #endregion

        #region ================================================================= METHODS =================================================================================== 
        /// <summary>
        /// Switches the current theme with the oposite theme
        /// </summary>
        public void SwitchTheme()
        {
            SetTheme(CurrentTheme switch
            {
                Themes.Dark => Themes.Light,
                Themes.Light => Themes.Dark,
                _ => throw new ArgumentOutOfRangeException(nameof(CurrentTheme))
            });
        }

        /// <summary>
        /// Sets the currently used theme
        /// </summary>
        /// <param name="theme">The theme to be set</param>
        public void SetTheme(Themes theme)
        {
            // change the first style in the main window styles section, and the main window instantly refreshes
            // (invoke only from the UI thread!)
            application.Styles[1] = theme switch
            {
                Themes.Dark => lightStyle,
                Themes.Light => darkStyle,
                _ => throw new ArgumentOutOfRangeException(nameof(theme))
            };
            CurrentTheme = theme;
        }

        /// <summary>
        /// Creates the style used for theming
        /// </summary>
        /// <param name="url">The url of the theme file to be used</param>
        /// <returns>The <see cref="StyleInclude"/> containing the styles inside the theme identified by <paramref name="url"/></returns>
        private static StyleInclude CreateStyle(string url)
        {
            return new StyleInclude(new Uri("resm:Styles?assembly=Leya.ViewsAvalonia"))
            {
                Source = new Uri(url)
            };
        }
        #endregion
    }
}