using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class DummyData
    {
        static readonly List<string> CustomerIDs =new List<string> { "Customer:1", "Customer:2", "Customer:3" };
        static readonly List<string> ContactNames = new List<string> { "John", "Smith", "David", "Joyce H. Noe", "Willie J.Rivers" };
        static readonly List<string> Countries = new List<string> { "America", "England", "Germany", };

        public static Customer GetDummyCustomer(string CustomerID = null)
        {
            Customer customer = new Customer();
            if (CustomerID==null)
            {
                CustomerID = CustomerIDs[GetRandomNumber(CustomerIDs.Count)];
            }
            customer.CustomerID = CustomerID;
            customer.ContactName = ContactNames[GetRandomNumber(ContactNames.Count)];
            customer.Country = Countries[GetRandomNumber(Countries.Count)];

            return customer;
        }
        private static int GetRandomNumber(int max)
        {
            Random random = new Random();
            return random.Next( max);            
        }
    }
}
