using Afleveringsopgave_3;
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
    /// Interaction logic for EnterFoodWindow.xaml
    /// </summary>
    public partial class EnterFoodWindow : Window
    {

        private bool nameInputIsValid = false;
        private bool categoryInputIsValid = false;
        private bool farmCo2InputIsValid = false;
        private bool ilucCo2InputIsValid = false;
        private bool processingCo2InputIsValid = false;
        private bool packagingCo2InputIsValid = false;
        private bool transportCo2InputIsValid = false;
        private bool retailCo2InputIsValid = false;
        private bool calculationIsValid = false;

        private double farmCo2InputValue = 0.0;
        private double ilucCo2InputValue = 0.0;
        private double processingCo2InputValue = 0.0;
        private double packagingCo2InputValue = 0.0;
        private double transportCo2InputValue = 0.0;
        private double retailCo2InputValue = 0.0;

        private FoodContainer _foodContainer;
        private MainWindow? _mainWindow;

        public EnterFoodWindow(FoodContainer foodContainer, MainWindow? mainWindow = null)
        {

            _foodContainer = foodContainer;
            _mainWindow = mainWindow;

            InitializeComponent();
            categoryInput.ItemsSource = Food.Categories;

            UpdateGui();
        }

        private void WindowContentLoaded(object sender, EventArgs e)
        {
            // Place in middle of parent window
            if (_mainWindow != null)
            {
                Left = _mainWindow.Left + _mainWindow.ActualWidth / 2 - ActualWidth / 2;
                Top = _mainWindow.Top + _mainWindow.ActualHeight / 2 - ActualHeight / 2;
            }
        }

        private void CalcTotalCO2ButtonClick(object sender, RoutedEventArgs e)
        {
            if (InputIsValid())
            {
                Food food = new Food
                (
                    name: nameInput.Text,
                    category: categoryInput.SelectedIndex,
                    co2FromFarming: farmCo2InputValue,
                    co2FromILUC: ilucCo2InputValue,
                    co2FromPackaging: packagingCo2InputValue,
                    co2FromProcessing: processingCo2InputValue,
                    co2FromTransport: transportCo2InputValue,
                    co2FromRetail: retailCo2InputValue
                );

                totalCo2Label.Content = food.Co2Total.ToString() + " kg/kg";
                calculationIsValid = true;
                UpdateGui();                
            }
        }

        private void SaveFoodButtonClick(object sender, RoutedEventArgs e)
        {
            if (InputIsValid())
            {
                Food food = new Food
                (
                    name: nameInput.Text,
                    category: categoryInput.SelectedIndex,
                    co2FromFarming: farmCo2InputValue,
                    co2FromILUC: ilucCo2InputValue,
                    co2FromPackaging: packagingCo2InputValue,
                    co2FromProcessing: processingCo2InputValue,
                    co2FromTransport: transportCo2InputValue,
                    co2FromRetail: retailCo2InputValue
                );

                _foodContainer.Add(food);                
                this.Close();
            }
        }


        private void NameInputTextChanged(object sender, TextChangedEventArgs e)
        {
            nameInputIsValid = Food.NameIsValid(nameInput.Text);

            if (nameInputIsValid)
            {
                nameInput.Background = Brushes.White;
            }
            else
            {
                nameInput.Background = Brushes.Yellow;
            }

            UpdateGui();
        }

        private void CategoryInputSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryInputIsValid = Food.CategoryIndexIsValid(categoryInput.SelectedIndex);
            UpdateGui();
        }

        private void FarmCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(farmCo2Input, out farmCo2InputValue, out farmCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void IlucCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(ilucCo2Input, out ilucCo2InputValue, out ilucCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void ProcessingCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(processingCo2Input, out processingCo2InputValue, out processingCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void PackagingCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(packagingCo2Input, out packagingCo2InputValue, out packagingCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void TransportCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(transportCo2Input, out transportCo2InputValue, out transportCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void RetailCo2InputTextChanged(object sender, TextChangedEventArgs e)
        {
            HandleTextBoxDoubleInput(retailCo2Input, out retailCo2InputValue, out retailCo2InputIsValid);
            calculationIsValid = false;
            UpdateGui();
        }

        private void UpdateGui()
        {
            calcTotalCO2Button.IsEnabled = InputIsValid();
            saveFoodButton.IsEnabled = InputIsValid();
            totalCo2Label.Visibility = calculationIsValid ? Visibility.Visible : Visibility.Hidden;
            categoryUnselectedLabel.Visibility = categoryInput.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
        }

        private bool InputIsValid()
        {
            return (
                nameInputIsValid &&
                categoryInputIsValid &&
                farmCo2InputIsValid &&
                ilucCo2InputIsValid &&
                processingCo2InputIsValid &&
                packagingCo2InputIsValid &&
                transportCo2InputIsValid &&
                retailCo2InputIsValid
            );
        }

        /// <summary>
        /// 
        /// Checks a textbox for a valid double input value.
        /// If the value is valid it will be written to the value parameter.
        /// If the value is invalid the textbox will turn yellow, and the out value will be set to 0.
        /// 
        /// </summary>
        private static void HandleTextBoxDoubleInput(TextBox textBox, out double value, out bool isValid)
        {
            double parseResult;
            isValid = double.TryParse(textBox.Text, out parseResult);
            if (isValid)
            {
                value = parseResult;
                textBox.Background = Brushes.White;
            }
            else
            {
                value = 0.0;
                textBox.Background = Brushes.Yellow;
            }
        }
    }
}
