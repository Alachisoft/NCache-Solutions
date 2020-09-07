using System;

namespace Alachisoft.NCache.Samples.Dapper.Models
{
    [Serializable]
    public class Customer
    {
        public Customer() { }

        public virtual string CustomerID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string Postalcode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }

    }
}
