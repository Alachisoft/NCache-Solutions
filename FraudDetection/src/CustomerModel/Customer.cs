// Copyright (c) 2019 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;

namespace CustomerSample
{

    [Serializable]
    ///
    /// This class contains information about customer 
    ///
    public class Customer
    {
        public Customer()
        { }

        /// <summary>
        /// Unique Id of the customer
        /// </summary>
        public int CustomerID
        {
            get;
            set;

        }

        public string EmailID
        {
            get;
            set;
        }

        /// <summary>
        /// Company the customer works for
        /// </summary>
        public  string CompanyName
        {
            set;
            get;
        }

        /// <summary>
        /// Contact number of the customer
        /// </summary>
        public string ContactNo
        {
            set;
            get;
        }

        /// <summary>
        /// Residential address of the customer
        /// </summary>
        public  string Address
        {
            set;
            get;
        }

        /// <summary>
        /// Residence city of the customer
        /// </summary>
        public  string City
        {
            set;
            get;
        }

        /// <summary>
        /// Nationality of the customer
        /// </summary>
        public  string Country
        {
            set;
            get;
        }

        /// <summary>
        /// Postal code of the customer
        /// </summary>
        public  string PostalCode
        {
            set;
            get;
        }

   
        public string  IPAdress
        {
            set;
            get;
        }

        public CardInfo CardInfo
        {
            set;
            get;
        }

        public Result TransactionResult
        {
            set;
            get;
        }

        public override bool Equals(object obj)
        {
            Customer customer = obj as Customer;
            if (customer!=null)
            {
                if (IPAdress.Equals(customer.IPAdress, StringComparison.InvariantCultureIgnoreCase)
                    || Country.Equals(customer.Country, StringComparison.InvariantCultureIgnoreCase) ||
                    City.Equals(customer.City, StringComparison.InvariantCultureIgnoreCase) ||
                    EmailID.Equals(customer.EmailID, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
           
        }
    }
}
    

