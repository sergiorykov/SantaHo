using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SantaHo.FrontEnd.ServiceContracts.Letters
{
    [DataContract]
    public class WishListLetterRequest
    {
        public WishListLetterRequest()
        {
            Wishes = new List<string>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> Wishes { get; set; }
    }
}
