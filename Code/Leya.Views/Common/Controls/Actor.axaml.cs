/// Written by: Yulia Danilova
/// Creation Date: 04th of December, 2020
/// Purpose: Code behind for the Actor user control
#region ========================================================================= USING =====================================================================================
using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
#endregion

namespace Leya.Views.Common.Controls
{
    public partial class Actor : UserControl
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly Label lblActorName;
        private readonly Label lblActorRole;
        private readonly Image imgActorImage;
        #endregion

        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DirectProperty<Actor, string> ActorNameProperty =
            AvaloniaProperty.RegisterDirect<Actor, string>(nameof(ActorName), e => e.ActorName, OnActorNameChanged);

        private string actorName;
        public string ActorName
        {
            get { return actorName; }
            set { SetAndRaise(ActorNameProperty, ref actorName, value); }
        }

        public static readonly DirectProperty<Actor, string> ActorRoleProperty =
            AvaloniaProperty.RegisterDirect<Actor, string>(nameof(ActorRole), e => e.ActorRole, OnActorRoleChanged);

        private string actorRole;
        public string ActorRole
        {
            get { return actorRole; }
            set { SetAndRaise(ActorRoleProperty, ref actorRole, value); }
        }

        public static readonly DirectProperty<Actor, string> ActorImageProperty =
            AvaloniaProperty.RegisterDirect<Actor, string>(nameof(ActorImage), e => e.ActorImage, OnActorImageChanged);

        private string actorImage;
        public string ActorImage
        {
            get { return actorImage; }
            set { SetAndRaise(ActorImageProperty, ref actorImage, value); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public Actor()
        {
            AvaloniaXamlLoader.Load(this);
            lblActorName = this.FindControl<Label>("lblActorName");
            lblActorRole = this.FindControl<Label>("lblActorRole");
            imgActorImage = this.FindControl<Image>("imgActorImage");
            DataContext = this;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Actor Name dependency property handler
        /// </summary>
        private static void OnActorNameChanged(Actor control, string value)
        {
            control.OnActorNameChanged(value);
        }

        /// <summary>
        /// Internal Actor Name dependency property handler
        /// </summary>
        private void OnActorNameChanged(string e)
        {
            lblActorName.Content = e;
        }

        /// <summary>
        /// Actor Role dependency property handler
        /// </summary>
        private static void OnActorRoleChanged(Actor control, string value)
        {
            control.OnActorRoleChanged(value);
        }

        /// <summary>
        /// Internal Actor Role dependency property handler
        /// </summary>
        private void OnActorRoleChanged(string e)
        {
            lblActorRole.Content = e;
        }

        /// <summary>
        /// Actor Image dependency property handler
        /// </summary>
        private static void OnActorImageChanged(Actor control, string value)
        {
            control.OnActorImageChanged(value);
        }

        /// <summary>
        /// Internal Actor Image dependency property handler
        /// </summary>
        private void OnActorImageChanged(string e)
        {
            IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            if (string.IsNullOrEmpty(e) || (!string.IsNullOrEmpty(e) && e == @"avares://Leya/Assets/no_actor.jpg"))
                imgActorImage.Source = new Bitmap(assets.Open(new Uri(@"avares://Leya/Assets/no_actor.jpg", UriKind.RelativeOrAbsolute)));
            else
                imgActorImage.Source = new Bitmap(e);
        }
        #endregion
    }
}
