using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ClientManagementServiceRTT.Models;
namespace ClientManagementServiceRTT
{
    [ServiceContract]
    public  interface IClientService
    {
        [OperationContract]
        int AddClient(string name, string gender, string details);

        [OperationContract]
        int AddEmp(Client client);

        [OperationContract]
        void AddAddress(int clientId, string addressType, string addressLine);

        [OperationContract]
        void AddContact(int clientId, string contactType, string contactValue);

        [OperationContract]
        Client GetClient(int clientId);

        [OperationContract]
        List<Client> GetAllClients();
    }

}
