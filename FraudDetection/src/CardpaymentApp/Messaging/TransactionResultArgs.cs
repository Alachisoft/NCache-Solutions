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

namespace CardPaymentApp.Messaging
{
    /// <summary>
    /// Result arguments for a transaction
    /// </summary>
   public  class TransactionResultArgs : EventArgs
    {
        public int CustomerID
        {
            get;set;
        }

        public string CustomerName
        {
            get; set;
        }

        public Result TransactionResult
        {
            get; set;
        }

        public string TransactionID
        { get; set; }
      
        public TransactionResultArgs(Result result, int customerId, string customerName, string transactionID)
        {
            CustomerID = customerId;
            TransactionResult = result;
            CustomerName = customerName;
            TransactionID = transactionID;
        }
    }
}
