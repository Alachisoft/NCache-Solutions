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

using CustomerSample;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerSample
{
    [Serializable]
    public class TransactionRequest
    {
        public int CustomerID
        {
            get; set;
        }

        public string City
        {
            get; set;
        }

        public string Country
        {
            get; set;
        }

        public string IPAdress
        {
            get; set;
        }
        public string EmailID
        {
            get; set;
        }
        public string CustomerName
        {
            get; set;
        }

        public long CardNumber
        {
            get; set;
        }
        public long TransactionAmount
        {
            get; set;
        }
        public string TransactionID
        { get; set; }
        
        public TransactionRequest()
        {
        }
        public TransactionRequest(int customerId, string city, string country, string ipAdress, string emailID, string customerName, long cardNumber, long transactionAmount)
        {
            CustomerID = customerId;
            City = city;
            Country = country;
            IPAdress = ipAdress;
            EmailID = emailID;
            CustomerName = customerName;
            CardNumber = cardNumber;
            TransactionAmount = transactionAmount;
        }

        public override bool Equals(object obj)
        {
            TransactionRequest transaction = obj as TransactionRequest;
            if (transaction != null)
            {
                if (IPAdress.Equals(transaction.IPAdress, StringComparison.InvariantCultureIgnoreCase)
                    || Country.Equals(transaction.Country, StringComparison.InvariantCultureIgnoreCase) ||
                    City.Equals(transaction.City, StringComparison.InvariantCultureIgnoreCase) ||
                    EmailID.Equals(transaction.EmailID, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;

        }
    }
    
}
