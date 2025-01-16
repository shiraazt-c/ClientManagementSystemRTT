using System;
using System.Collections.Generic;
using System.Linq;
// AddClientWindow.xaml.cs
using ClientManagementServiceRTT.Models;
using System.Windows;

namespace MyWpfApp
{
    public partial class AddClientWindow : Window
    {
        public ClientManagementServiceRTT.Models.Client NewClient { get; private set; }

        public AddClientWindow()
        {
            InitializeComponent();
            NewClient = new ClientManagementServiceRTT.Models.Client
            {
                Addresses = new List<ClientManagementServiceRTT.Models.Address>(),
                Contacts = new List<ClientManagementServiceRTT.Models.Contact>(),
                Details = string.Empty
            };
            LoadGenderComboBox(); // Add this line to call LoadGenderComboBox method
        }

        private void LoadGenderComboBox()
        {
            var genders = new Dictionary<string, string>
                    {
                        { "M", "Male" },
                        { "F", "Female" }
                    };

            GenderComboBox.ItemsSource = genders;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(GenderComboBox.Text) || string.IsNullOrWhiteSpace(DetailsTextBox.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewClient.Name = NameTextBox.Text;
            NewClient.Gender = GenderComboBox.Text.Substring(0, 1);
            NewClient.Details = DetailsTextBox.Text;

            DialogResult = true;
        }

        private void AddAddressButton_Click(object sender, RoutedEventArgs e)
        {
            var addAddressWindow = new AddAddressWindow();
            if (addAddressWindow.ShowDialog() == true)
            {
                NewClient.Addresses.Add(addAddressWindow.NewAddress);
            }
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            var addContactWindow = new AddContactWindow();
            if (addContactWindow.ShowDialog() == true)
            {
                NewClient.Contacts.Add(addContactWindow.NewContact);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

