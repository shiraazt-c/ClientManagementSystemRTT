using ClientManagementServiceRTT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagementServiceRTT.Data
{
    public class ClientManagementBusiness
    {
        private readonly ClientRepository _clientRepository;

        public ClientManagementBusiness(string connectionString)
        {
            _clientRepository = new ClientRepository(connectionString);
        }

        public int AddClient(string name, string gender, string details)
        {
            return _clientRepository.AddClient(name, gender, details);
        }

        public void AddAddress(int clientId, string addressType, string addressLine)
        {
            _clientRepository.AddAddress(clientId, addressType, addressLine);
        }

        public void AddContact(int clientId, string contactType, string contactValue)
        {
            _clientRepository.AddContact(clientId, contactType, contactValue);
        }
        public Client GetClient(int clientId)
        {
            // Interact with the data access layer to retrieve client data
            var client = _clientRepository.GetClient(clientId);

            // Optionally, add business rules or transformations here
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {clientId} does not exist.");
            }

            return client;
        }

        public List<Client> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            return clients.ToList();

        }
    }
}
