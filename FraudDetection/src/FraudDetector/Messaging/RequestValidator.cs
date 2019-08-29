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

using Alachisoft.NCache.Runtime.Caching;
using CustomerSample;
using FraudDetector.NCacheLogic;
using FraudDetector.TrainedLogic;
using System.Collections.Generic;

namespace FraudDetector.Messaging
{
    public class RequestValidator
    {
        CacheHandler ncache;
        LearntLogic learntLogic; // learnt logic of traineddata
        public RequestValidator(CacheHandler cache)
        {
            ncache = cache;
            learntLogic = new LearntLogic(cache);
        }

        public Result PerformTransaction(TransactionRequest transactionMessage)
        {
            // this miodel is in memory, you can update this logic accordingly
            try
            {
                return PerformTransactionOnLocalMemory(transactionMessage);
            }
            catch
            {
                throw;
            }
        }

        Result PerformTransactionOnLocalMemory(TransactionRequest transactionMessage)
        {
            // first data is obtained from cache and it is checked if any fraud or suspicious transaction was made
            // if yes than this transaction is declared as failure
            // if transaction is not present in local memory than a trained model is run on it. 
            // which declares results, which are sent back to dfraud manager
            // this is a demo logic you can insert your code here
            Result result = Result.Valid;
            try
            {
                if (transactionMessage != null)
                {
                    string customerkey = transactionMessage.CustomerID.ToString();
                    IList<FraudRequest> customerInfo = ncache.FetchDataFromCache(customerkey);
                    if (customerInfo != null) // if there was already a failed transaction mark th transaction failure
                    {
                        if (learntLogic.FraudFoundInLastTransactions(transactionMessage, customerInfo))
                        {
                            result = Result.Faliure;
                        }
                    }
                    if (result != Result.Faliure)
                    {
                        bool isValid = learntLogic.IsTransactionValid(transactionMessage); // if transaction is not valid, mark it as suspicious
                        if (!isValid)
                            result = Result.Suspicious;
                    }
                    FraudRequest fraudRequest = CreateFraudRequest(transactionMessage, result);
                    
                    ncache.UpdateCustomerInfoInCache(customerkey, fraudRequest);
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
        private FraudRequest CreateFraudRequest (TransactionRequest transactionRequest , Result result)
        {
            FraudRequest fraudRequest = new FraudRequest();
            fraudRequest.CardNumber = transactionRequest.CardNumber;
            fraudRequest.City = transactionRequest.City;
            fraudRequest.Country = transactionRequest.Country;
            fraudRequest.CustomerID= transactionRequest.CustomerID;
            fraudRequest.CustomerName = transactionRequest.CustomerName;
            fraudRequest.EmailID = transactionRequest.EmailID;
            fraudRequest.IPAdress = transactionRequest.IPAdress;
            fraudRequest.TransactionAmount = transactionRequest.TransactionAmount;
            fraudRequest.TransactionResult = result;

            return fraudRequest;
        }

    }
}
    