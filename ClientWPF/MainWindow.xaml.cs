using Binance.Net.ClientWPF.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Binance.Net.ClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Grid_Updated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if ((sender as ListView)?.View is GridView gridview)
            {
                foreach (var column in gridview.Columns)
                {
                    // Set the Width. Then clear it to cause the autosize.
                    column.Width = 1;
                    column.ClearValue(GridViewColumn.WidthProperty);
                }
            }
        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        private void SortClick(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var header = e.OriginalSource as GridViewColumnHeader;
            string sortBy = header.Content.ToString();
            header.Tag = header.Content;
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                listView.Items.SortDescriptions.Clear();
            }
        
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == header && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;
        
            listViewSortCol = header;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            listView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }
}
