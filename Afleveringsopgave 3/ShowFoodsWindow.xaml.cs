using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Afleveringsopgave_3
{
    /// <summary>
    /// Interaction logic for ShowFoodsWindow.xaml
    /// </summary>
    public partial class ShowFoodsWindow : Window
    {
        private FoodContainer _foodContainer;
        private Window? _parentWindow;


        public ShowFoodsWindow(FoodContainer foodContainer, Window? parentWindow = null)
        {
            _foodContainer = foodContainer;
            _parentWindow = parentWindow;
            InitializeComponent();          
            
            dataGrid.ItemsSource = _foodContainer.List;
            dataGrid.AutoGenerateColumns = false; // Manually control columns to remove category index and have prettier headers
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Name", Binding = new Binding(nameof(Food.Name))});
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Category", Binding = new Binding(nameof(Food.CategoryString))});
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Farming CO2", Binding = new Binding(nameof(Food.Co2FromFarmingFormatted))});
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "ILUC CO2", Binding = new Binding(nameof(Food.Co2FromILUCFormatted))});
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Processing CO2", Binding = new Binding(nameof(Food.Co2FromProcessingFormatted)) });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Packaging CO2", Binding = new Binding(nameof(Food.Co2FromPackagingFormatted)) });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Transport CO2", Binding = new Binding(nameof(Food.Co2FromTransportFormatted)) });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Retail CO2", Binding = new Binding(nameof(Food.Co2FromRetailFormatted)) });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Total CO2", Binding = new Binding(nameof(Food.Co2TotalFormatted)) });
            dataGrid.IsReadOnly = true; // No editing from grid. Otherwise if cell is clicked it will throw error.
        }

        private void WindowContentLoaded(object sender, EventArgs e)
        {
            // Place in middle of parent window
            if (_parentWindow != null)
            {
                Left = _parentWindow.Left + _parentWindow.ActualWidth / 2 - ActualWidth / 2;
                Top = _parentWindow.Top + _parentWindow.ActualHeight / 2 - ActualHeight / 2;
            }
        }

    }
}
