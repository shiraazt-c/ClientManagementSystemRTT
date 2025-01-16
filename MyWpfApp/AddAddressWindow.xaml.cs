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
using ClientManagementServiceRTT.Models;
namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for AddAddressWindow.xaml
    /// </summary>
    public partial class AddAddressWindow : Window
    {
        public Address NewAddress { get; private set; }

        public AddAddressWindow()
        {
            InitializeComponent();
            NewAddress = new Address();
            LoadAddressTypeComboBox();
        }

        private void LoadAddressTypeComboBox()
        {
            var addressTypes = new Dictionary<string, string>
            {
                { "ResidentialAddress", "Residential Address" },
                { "WorkAddress", "Work Address" },
                { "PostalAddress", "Postal Address" },
                { "CellNumber", "Cell Number" },
                { "WorkNumber", "Work Number" }
            };

            // Bind the dictionary to the ComboBox
            AddressTypeComboBox.ItemsSource = addressTypes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTypeComboBox.Text) || string.IsNullOrWhiteSpace(AddressLineTextBox.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewAddress.AddressType = AddressTypeComboBox.Text;
            NewAddress.AddressLine = AddressLineTextBox.Text;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
