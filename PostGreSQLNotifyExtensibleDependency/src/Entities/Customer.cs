using System;

// PostGreSQLReadThruProvider.cs constructs Customer in LoadFromSource.
namespace Entities
{
    [Serializable]
    public class Customer
    {
        public Customer()
        {
        }

        public string customerid { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string address { get; set; }
    }
}
