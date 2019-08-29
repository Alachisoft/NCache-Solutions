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
using FraudDetector.NCacheLogic;
using System.Collections.Generic;
using System.Net;

namespace FraudDetector.TrainedLogic
{
    /// <summary>
    /// this class is dummy class and contains a dummy logic of Ai
    /// You can replace it with your own logic
    ///
    /// </summary>
    public class LearntLogic
    {
        private int fraudWeight = 0;
        private const int decisionWeight = 3;
        object lockObject = new object();

        /// faud structures 
        /// 

        CacheHandler learntCache;
        public LearntLogic(CacheHandler cache)
        {
            learntCache = cache;
            InitililiazeData();
        }
        // increment threadsafe fraud weight
        void IncrementFraudWeight ()
        {
            lock (lockObject)
            {
                fraudWeight++;
            }
        }

        // resets weight to zero
        void ReSetFraudWeight()
        {
            lock (lockObject)
            {
                fraudWeight =0;
            }
        }
        void InitililiazeData()
        {
            // this data should come from trained model. 
            // this is dummy data
            learntCache.UpdateTrainedDataInCache("fraudlentCountries", "Ukraine");
            learntCache.UpdateTrainedDataInCache("fraudlentCountries", "Indonesia");
            learntCache.UpdateTrainedDataInCache("fraudlentCountries", "Yugoslavia");

            learntCache.UpdateTrainedDataInCache("fraudlentCities", "abc");
            learntCache.UpdateTrainedDataInCache("fraudlentCities", "def");
            learntCache.UpdateTrainedDataInCache("fraudlentCities", "gh");

            learntCache.UpdateTrainedDataInCache("fraudlentEmail", "abc@gmail.com");

            learntCache.UpdateTrainedDataInCache("invalidEmailDoamin", "abc.com");
            learntCache.UpdateTrainedDataInCache("invalidEmailDoamin", "lmbn.com");
        }

       public bool IsTransactionValid(TransactionRequest transaction)
        {
            // you can replace this logic with yours
            // if certain criterias are net met, increment the fraud weight. 
            // if fraud weight is greater than deciosn weight, declare the transaction as fraud

            bool isvalidTransaction = true;
            ReSetFraudWeight();

            if (transaction.CardNumber ==0 )
                isvalidTransaction =  false; // empty card info indicates fraud
            else
            {
                if (!IsValidIPAddress(transaction.IPAdress))
                    IncrementFraudWeight();
                if (!IsValidEmail(transaction.EmailID))
                    IncrementFraudWeight();
                if (!IsCountryFraudlent(transaction.Country))
                    IncrementFraudWeight();
                if (!IsCityFraudlent(transaction.City))
                    IncrementFraudWeight();

                if (fraudWeight >= decisionWeight)
                {
                    isvalidTransaction = false; 
                }
            }
            return isvalidTransaction; 
        }

        bool IsValidIPAddress(string address)
        {
            bool validAddress = true;
            IPAddress ipAddress;

            if (string.IsNullOrEmpty(address))
                validAddress = false;
            else
            {
                validAddress = IPAddress.TryParse(address, out ipAddress);

                // here you can add your buisness logic. 
                //you can also check if ip lien in the domain of the country where it belongs
            }
            return validAddress;

        }
        bool IsCityFraudlent(string city)
        {
            bool validCity = true;

            if (string.IsNullOrEmpty(city))
            {
                validCity = false;
                return validCity;
            }
            else
            {

                IList<string> fraudlentCities = learntCache.GetTrainedDataFromCache("fraudlentCities");
                if (fraudlentCities.Contains(city))
                {
                    validCity = false;
                    // here you can add your buisness logic. 
                }
                return validCity;
            }
        }
        bool IsCountryFraudlent (string country)
        {
            bool validCountry = true;

            if (string.IsNullOrEmpty(country))
                validCountry = false;
            else 
            {
                IList<string> fraudlentCountries = learntCache.GetTrainedDataFromCache("fraudlentCountries");
                if (fraudlentCountries.Contains(country)) validCountry = false;
                // here you can add your buisness logic. 
            }
            return validCountry;

        }
        bool IsValidEmail(string email)
        {
            bool validEmail = true;

            if (string.IsNullOrEmpty(email))
            {
                validEmail = false; // empty email indicates fraud
            }
            else if (!email.Contains('@'))
            {
                validEmail = false;
            }

            IList<string> invalidEmailDoamin = learntCache.GetTrainedDataFromCache("invalidEmailDoamin");

            if (invalidEmailDoamin.Contains(email))
                validEmail = false;
            IList<string> fraudlentEmail = learntCache.GetTrainedDataFromCache("fraudlentEmail");
            if (fraudlentEmail.Contains(email))
                validEmail = false;
            // you can add ypur buisness logic here for email validation 
            return validEmail;
        }
        public bool FraudFoundInLastTransactions(TransactionRequest pendingTransaction, IList<FraudRequest> prevTransactions)
        {
            // verify if there was a fraud in last transactions, if yes than this may be a fraud ior suspicios transaction
            bool lastFradulentTransactionsExists = false;

            if (prevTransactions != null && prevTransactions.Count > 0)
            {
               foreach (FraudRequest transaction in prevTransactions)
                {
                    if (transaction.TransactionResult == Result.Suspicious || transaction.TransactionResult == Result.Faliure)
                    {
                        if (transaction.Equals(pendingTransaction))
                        {
                            lastFradulentTransactionsExists = true;
                            break;
                        }
                    }
                }
            }

            return lastFradulentTransactionsExists;
        }
    }
}
