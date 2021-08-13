using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Leya.Views.Options
{
    public partial class OptionsMediaV : Window
    {
        public OptionsMediaV()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
