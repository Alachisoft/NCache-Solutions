// Copyright (c) 2020 Alachisoft
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

using Newtonsoft.Json;
using System;

namespace Alachisoft.NCache.Samples
{
    // The Customer entity class will be used to simulate customer data being added to the CosmosDb SQL database and cache and 
    // demonstrating dependency invocation using CosmosDbNotificationDependency
    [Serializable]
    public class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "ContactName")]
        public string ContactName { get; set; }

        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }


        public Customer()
        { }

        public Customer(string cid, string name, string company, string address, string city, string country)
        {
            this.Id = cid;
            this.ContactName = name;
            this.CompanyName = company;
            this.Address = address;
            this.City = city;
            this.Country = country;
        }

        public override string ToString()
        {
            return $"CustomerID:\t\t{Id}" +
                    $"\nContactName:\t\t{ContactName}" +
                    $"\nCompany:\t\t{CompanyName}" +
                    $"\nAddress:\t\t{Address}" +
                    $"\nCity:\t\t\t{City}" +
                    $"\nCountry:\t\t{Country}";
        }

    }
}
