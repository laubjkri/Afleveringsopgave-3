using KL_Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Afleveringsopgave_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CsvReader _reader = new CsvReader();
        private FoodContainer _foodContainer = new();

        public MainWindow()
        {
            
            _reader.PopupEvent += ShowInfo;
            _reader.LogEvent += Log;
            _foodContainer.LogEvent += Log;

            InitializeComponent();
            ClearLog();
        }

        private void EnterFoodButtonClick(object sender, RoutedEventArgs e)
        {
            EnterFoodWindow enterFoodWindow = new EnterFoodWindow(_foodContainer, this);
            enterFoodWindow.Show();
        }

        private async void ReadFileButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "csv files|*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    _reader.FilePath = openFileDialog.FileName;
                    string fileName = System.IO.Path.GetFileName(_reader.FilePath);
                    Log($"File \"{fileName}\" selected.");

                    if (await _reader.ReadFileLinesExclusive())
                    {                        
                        int foodsAdded = 0;
                        foreach (CsvReaderRow row in _reader.Rows)
                        {
                            try
                            {
                                Food food = new Food
                                (
                                    name: row.GetColumnString("Navn"),
                                    category: row.GetColumnString("DSK Kategori"),
                                    co2FromFarming: row.GetColumnDouble("Agriculture"),
                                    co2FromILUC: row.GetColumnDouble("iLUC"),
                                    co2FromPackaging: row.GetColumnDouble("Packaging"),
                                    co2FromProcessing: row.GetColumnDouble("Food processing"),
                                    co2FromTransport: row.GetColumnDouble("Transport"),
                                    co2FromRetail: row.GetColumnDouble("Retail")
                                );

                                _foodContainer.Add(food);
                                foodsAdded++;

                            }
                            catch (Exception ex)
                            {
                                Log($"Error on row {row.RowIndex}: {ex.Message}");
                            }
                        }

                        Log($"{foodsAdded} foods added from file.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ClearLogButtonClick(object sender, RoutedEventArgs e)
        {
            ClearLog();
        }


        public void Log(string message)
        {
            logTextBox.AppendText(message + '\n');
        }

        private void ClearLog()
        {
            logTextBox.Clear();
        }

        private void ShowError(string message)
        {
           KL_Utils.MessageBox.Show("Error", message, this);
        }

        private void ShowInfo(string message)
        {
            KL_Utils.MessageBox.Show("Info", message, this);
        }

        private void ShowFoodsButtonClick(object sender, RoutedEventArgs e)
        {
            ShowFoodsWindow showFoodsWindow = new ShowFoodsWindow(_foodContainer, this);
            showFoodsWindow.Show();
        }
    }
}
