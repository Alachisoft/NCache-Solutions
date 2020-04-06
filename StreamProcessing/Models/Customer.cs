using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class Customer: EntityBase
    {
        public string Company { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<Order> Orders { get; set; }

        public int OrdersCount { get; set; }

        public override string ToString()
        {
            return $"CustomerID:\t\t{Id}" +
                    $"\nContactName:\t\t{Contact}" +
                    $"\nCompany:\t\t{Company}" +
                    $"\nAddress:\t\t{Address}" +
                    $"\nCity:\t\t\t{City}" +
                    $"\nCountry:\t\t{Country}";
        }
    }
}
