using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagementServiceRTT.Models
{
    [DataContract]
    public class Client
    {
        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public List<Address> Addresses { get; set; } 

        [DataMember]
        public List<Contact> Contacts { get; set; } 
    }
}
