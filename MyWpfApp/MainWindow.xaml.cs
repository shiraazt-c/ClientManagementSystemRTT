// MainWindow.xaml.cs
using ClientManagementServiceRTT.MyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ClientManagementServiceRTT.Data;
using ClientManagementServiceRTT.Models;

namespace MyWpfApp
{
    public partial class MainWindow : Window
    {
        private readonly ClientServiceClient _clientService;
        private string _connectionString = "Server=DESKTOP-41RHM0I\\SQLEXPRESS;database=shiraaztest;trusted_connection=true;Encrypt=False;TrustServerCertificate=True;";


        public MainWindow()
        {
            InitializeComponent();
            _clientService = new ClientServiceClient();
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                // Fetch all clients from the service
                List<ClientManagementServiceRTT.MyService.Client> clients = _clientService.GetAllClients();

                ClientsListView.ItemsSource = clients;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }

        private void CreateClientButton_Click(object sender, RoutedEventArgs e)
        {
            var addClientWindow = new AddClientWindow();
            if (addClientWindow.ShowDialog() == true)
            {
                try
                {
                    var newClient = new ClientManagementServiceRTT.MyService.Client
                    {
                        ClientId = addClientWindow.NewClient.ClientId,
                        Name = addClientWindow.NewClient.Name,
                        Gender = addClientWindow.NewClient.Gender,
                        Details = addClientWindow.NewClient.Details,
                        Addresses = addClientWindow.NewClient.Addresses.Select(a => new ClientManagementServiceRTT.MyService.Address
                        {
                            AddressId = a.AddressId,
                            AddressLine = a.AddressLine,
                            AddressType = a.AddressType
                        }).ToList(),
                        Contacts = addClientWindow.NewClient.Contacts.Select(c => new ClientManagementServiceRTT.MyService.Contact
                        {
                            ContactId = c.ContactId,
                            ContactType = c.ContactType,
                            ContactValue = c.ContactValue
                        }).ToList()
                    };
                    _clientService.AddEmp(newClient);
                    LoadClients();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error adding client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

            {

                // Simulated data fetching - Replace with your actual data retrieval logic
                //  var clients = GetClientsWithAddresses();
                try
                {
                    ClientRepository repository = new ClientRepository(_connectionString);
                    var clients = repository.GetClientsWithAddresses();
                    // File dialog to select save location
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "CSV files (*.csv)|*.csv",
                        FileName = "ClientsExport.csv",
                        DefaultExt = ".csv"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var filePath = saveFileDialog.FileName;

                        // Write to CSV file
                        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine("ClientId,Name,Gender,AddressType,AddressLine"); // Header

                            foreach (var client in clients)
                            {
                                foreach (var address in client.Addresses)
                                {
                                    writer.WriteLine($"{client.ClientId},{client.Name},{client.Gender},{address.AddressType},{address.AddressLine}");
                                }
                            }
                        }

                        MessageBox.Show("Export successful!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during export: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //private List<Client> GetClientsWithAddresses()
            //{
            //    // Simulated clients and addresses for demonstration
            //    return new List<Client>
            //    {
            //        new Client
            //        {
            //            ClientId = 1,
            //            Name = "John Doe",
            //            Gender = "M",
            //            Addresses = new List<Address>
            //            {
            //                new Address { AddressType = "Residential", AddressLine = "123 Main St" },
            //                new Address { AddressType = "Work", AddressLine = "456 Office Rd" }
            //            }
            //        },
            //        new Client
            //        {
            //            ClientId = 2,
            //            Name = "Jane Smith",
            //            Gender = "F",
            //            Addresses = new List<Address>
            //            {
            //                new Address { AddressType = "Postal", AddressLine = "789 PO Box" }
            //            }
            //        }
            //    };
            //}
        }
    }
}
