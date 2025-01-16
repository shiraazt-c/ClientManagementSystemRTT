using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using ClientManagementServiceRTT.Models;
using System.Threading.Tasks;
using ClientManagementServiceRTT.Data;


namespace ClientManagementServiceRTT
{
    public class ClientService : IClientService
    {
        private readonly string _connectionString = "Server=DESKTOP-41RHM0I\\SQLEXPRESS;database=shiraaztest;trusted_connection=true;Encrypt=False;TrustServerCertificate=True;";

        private readonly ClientManager _clientManager;

        public ClientService()
        {
            // Connection string can be moved to a configuration file
           // string connectionString = "Server=DESKTOP-41RHM0I\\SQLEXPRESS;database=shiraaztest;trusted_connection=true;Encrypt=False;TrustServerCertificate=True;";
            _clientManager = new ClientManager(_connectionString);
        }

        public int AddClient(string name, string gender, string details)
        {
            return _clientManager.AddClient(name, gender, details);
        }

        public int AddEmp(Client client)
        {
            return _clientManager.AddEmp(client);
        }


        public void AddAddress(int clientId, string addressType, string addressLine)
        {
            _clientManager.AddAddress(clientId, addressType, addressLine);
        }

        public void AddContact(int clientId, string contactType, string contactValue)
        {
            _clientManager.AddContact(clientId, contactType, contactValue);
        }

        public Client GetClient(int clientId)
        {
            try
            {
                return _clientManager.GetClient(clientId);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a custom fault
                Console.WriteLine($"Error in GetClient: {ex.Message}");
                throw;
            }
        }
         public List<Client> GetAllClients()
        {
            var clientArray = _clientManager.GetAllClients();
            return clientArray.ToList();
        }


    }
}
