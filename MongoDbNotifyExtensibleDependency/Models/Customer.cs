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

using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models
{
    // The Customer entity class will be used to simulate customer data being added to the MongoDB database and cache and 
    // demonstrating dependency invocation using MongoDbNotificationDependency

    [Serializable]
    [BsonIgnoreExtraElements]
    public class Customer
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return 
                $"Customer ID: {CustomerId}\n" +
                $"Contact Name: {ContactName}\n" +
                $"Company Name: {CompanyName}\n" +
                $"Address: {Address}\n" +
                $"City: {City}\nCountry: {Country}";
        }
    }
}
