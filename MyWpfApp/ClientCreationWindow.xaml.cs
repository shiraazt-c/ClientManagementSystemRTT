using System.Collections.Generic;
using System.Windows;

namespace MyWpfApp
{
    public partial class ClientCreationWindow : Window
    {
        public ClientCreationWindow()
        {
            InitializeComponent();
            LoadGenderComboBox();
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
            var name = ClientNameTextBox.Text;
            var gender = GenderComboBox.SelectedValue as string;
            var details = ClientDetailsTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Please fill out all required fields.");
                return;
            }

            // Logic to save the client
            MessageBox.Show($"Client Saved:\nName: {name}\nGender: {gender}\nDetails: {details}");

            // Close the window or clear the form
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without saving
            this.Close();
        }
    }
}
