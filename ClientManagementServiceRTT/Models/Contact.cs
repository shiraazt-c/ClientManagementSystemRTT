using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagementServiceRTT.Models
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public int ContactId { get; set; }

        [DataMember]
        public string ContactType { get; set; }

        [DataMember]
        public string ContactValue { get; set; }
    }
}
