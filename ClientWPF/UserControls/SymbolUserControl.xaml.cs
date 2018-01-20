using System.Windows.Controls;

namespace Binance.Net.ClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for SymbolUserControl.xaml
    /// </summary>
    public partial class SymbolUserControl : UserControl
    {
        public SymbolUserControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
