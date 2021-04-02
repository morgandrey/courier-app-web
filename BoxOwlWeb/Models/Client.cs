using System;
using System.Collections.Generic;

namespace BoxOwlWeb.Models
{
    public partial class Client
    {
        public Client()
        {
            Order = new HashSet<Order>();
        }

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPassword { get; set; }
        public string ClientSalt { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
