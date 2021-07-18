/// Written by: Yulia Danilova
/// Creation Date: 04th of December, 2020
/// Purpose: Code behind for the Actor user control
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
#endregion

namespace Leya.Views.Common.Controls
{
    /// <summary>
    /// Interaction logic for Actor.xaml
    /// </summary>
    public partial class Actor : UserControl, INotifyPropertyChanged
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region ========================================================== DEPENDENCY PROPERTIES ============================================================================
        public static readonly DependencyProperty ActorNameProperty = DependencyProperty.Register("ActorName", typeof(string), typeof(Actor),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnActorNameChanged)));
        public string ActorName
        {
            get { return (string)GetValue(ActorNameProperty); }
            set { SetValue(ActorNameProperty, value); }
        }

        public static readonly DependencyProperty ActorRoleProperty = DependencyProperty.Register("ActorRole", typeof(string), typeof(Actor),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnActorRoleChanged)));
        public string ActorRole
        {
            get { return (string)GetValue(ActorRoleProperty); }
            set { SetValue(ActorRoleProperty, value); }
        }

        public static readonly DependencyProperty ActorImageProperty = DependencyProperty.Register("ActorImage", typeof(string), typeof(Actor),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnActorImageChanged)));
        public string ActorImage
        {
            get { return (string)GetValue(ActorImageProperty); }
            set { SetValue(ActorImageProperty, value); }
        }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public Actor()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Notifies the UI about a binded property's value being changed
        /// </summary>
        /// <param name="propertyName">The property that had the value changed</param>
        public void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Actor Name dependency property handler
        /// </summary>
        private static void OnActorNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Actor control = d as Actor;
            control.OnActorNameChanged(e);
        }

        /// <summary>
        /// Internal Actor Name dependency property handler
        /// </summary>
        private void OnActorNameChanged(DependencyPropertyChangedEventArgs e)
        {
            mrqActorName.SetText = e.NewValue.ToString();
        }

        /// <summary>
        /// Actor Role dependency property handler
        /// </summary>
        private static void OnActorRoleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Actor control = d as Actor;
            control.OnActorRoleChanged(e);
        }

        /// <summary>
        /// Internal Actor Role dependency property handler
        /// </summary>
        private void OnActorRoleChanged(DependencyPropertyChangedEventArgs e)
        {
            mrqActorRole.SetText = e.NewValue.ToString();
        }

        /// <summary>
        /// Actor Image dependency property handler
        /// </summary>
        private static void OnActorImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Actor control = d as Actor;
            control.OnActorImageChanged(e);
        }

        /// <summary>
        /// Internal Actor Image dependency property handler
        /// </summary>
        private void OnActorImageChanged(DependencyPropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewValue.ToString()))
                imgActorImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Leya;component/Resources/no_actor.jpg", UriKind.Absolute));
            else
                imgActorImage.Source = new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.Absolute));
        }
        #endregion
    }
}