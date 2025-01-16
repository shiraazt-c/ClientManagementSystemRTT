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
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public Contact NewContact { get; private set; }

        public AddContactWindow()
        {
            InitializeComponent();
            LoadContactTypeComboBox();
            NewContact = new Contact();
        }

        private void LoadContactTypeComboBox()
        {
            var contactTypes = new Dictionary<string, string>
            {
               { "Email", "Email" },
                { "Phone", "Phone" },
                { "Fax", "Fax" },
                { "Skype", "Skype" }
            };

            ContactTypeComboBox.ItemsSource = contactTypes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ContactTypeComboBox.Text) || string.IsNullOrWhiteSpace(ContactValueTextBox.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewContact.ContactType = ContactTypeComboBox.Text;
            NewContact.ContactValue = ContactValueTextBox.Text;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

