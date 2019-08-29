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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CustomerSample
{
    /// <summary>
    /// This class contains information for sql connectivity and multiple methods for database
    /// </summary>
    public class SqlDataSource
    {
        private SqlConnection sqlConnection;
        private string connString;
        
        /// <summary>
        /// Returns the connection string of this datasource.
        /// </summary>
        public string ConnString { get { return connString; } }

        /// <summary>
        /// Establish connection with the datasource.
        /// </summary>
        /// <param name="connString"></param>
        public void Connect(string connString)
        {
            if (connString != "")
                this.connString = connString;
            sqlConnection = new SqlConnection(this.connString);
            sqlConnection.Open();
        }

        /// <summary>
        /// Releases the connection.
        /// </summary>
        public void DisConnect()
        {
            if (sqlConnection != null)
                sqlConnection.Close();
        }


        public Customer LoadCustomersCardInfo(long cardNumber)
        {
            string command = "SELECT CustomerName,CardType,CardNumber,CardStartDate,CardExpiryDate,CardLimit FROM Customers FULL OUTER JOIN CardInfo ON Customers.CustomerNo = CardInfo.CustomerNo";
            if (cardNumber!=-1) command= $"{command} WHERE CardInfo.CardNumber = { cardNumber.ToString()}";
            SqlCommand cmd = GetCommad(command);
            return GetCustomerInfo(cmd);
            
        }
        public SqlCommand GetCommad(string commandtext)
        {
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = commandtext;
            return cmd;
        }
        public Customer GetCustomerInfo(SqlCommand sqlCommad)
        {
            try
            {
                SqlDataReader reader = sqlCommad.ExecuteReader();
                Customer objCustomer = new Customer();
                objCustomer.CardInfo = new CardInfo();

                if (reader.Read())
                {

                    if (!reader.IsDBNull(0))
                    {
                        objCustomer.CardInfo.CardHolder = reader.GetString(0) ?? "";
                    }

                    if (!reader.IsDBNull(1))
                    {
                        objCustomer.CardInfo.CardType = GetCardType(reader.GetString(1));
                    }

                    if (!reader.IsDBNull(2))
                    {

                        objCustomer.CardInfo.CardNumber = Convert.ToInt64(reader.GetString(2));
                    }
                    if (!reader.IsDBNull(3))
                    {
                        objCustomer.CardInfo.CardStartDate = Convert.ToDateTime(reader.GetString(3));
                    }
                    if (!reader.IsDBNull(4))
                    {
                        objCustomer.CardInfo.CardExpiryDate = Convert.ToDateTime(reader.GetString(4));
                    }
                    if (!reader.IsDBNull(5))
                    {
                        objCustomer.CardInfo.CardLimit = Convert.ToInt64(reader.GetString(5));
                    }
                }
                reader.Close();
                return objCustomer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public FraudRequest LoadPreviouisTransactions(SqlCommand sqlCommad)
        {
            SqlDataReader reader = null;
            try
            {
                reader = sqlCommad.ExecuteReader();
                FraudRequest objCustomer = new FraudRequest();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(1))
                    {
                        objCustomer.IPAdress = reader.GetString(1) ?? "";
                    }
                    if (!reader.IsDBNull(2))
                    {
                        objCustomer.TransactionAmount = Convert.ToInt64(reader.GetString(2));
                    }

                    if (!reader.IsDBNull(3))
                    {
                        objCustomer.TransactionResult = GetTransactionResult(reader.GetString(3));
                    }
                    if (!reader.IsDBNull(4))
                    {
                        objCustomer.CustomerID = Convert.ToInt32(reader.GetString(4));
                    }
                    if (!reader.IsDBNull(5))
                    {
                        objCustomer.CardNumber = reader.GetInt64(5);
                    }

                    if (!reader.IsDBNull(6))
                    {
                        objCustomer.City = reader.GetString(6) ?? "";
                    }
                    if (!reader.IsDBNull(7))
                    {
                        objCustomer.Country = reader.GetString(7) ?? "";
                    }
                    if (!reader.IsDBNull(8))
                    {
                        objCustomer.CardNumber = Convert.ToInt64(reader.GetString(8));
                    }

                }
                reader.Close();
                return objCustomer;
            }
            finally
            {
                reader.Close();
            }
        }
        public bool SaveTransaction(FraudRequest transactionMade)
        {
            int rowsChanged = 0;
            string[] transaction =  {transactionMade.IPAdress,transactionMade.TransactionAmount.ToString(),transactionMade.TransactionResult.ToString(),
                                    transactionMade.CustomerID.ToString(),transactionMade.CardNumber.ToString(),transactionMade.City,transactionMade.Country,
                                    transactionMade.EmailID};

            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                             "Insert into dbo.Transactions" +
                                            "(IPAdress,TransactionAmount,Result,CustomerNo,CardNumber,City,Country,Email)" +
                                            "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", transaction );
            rowsChanged = cmd.ExecuteNonQuery();
            if (rowsChanged > 0)
            {
                return true;
            }
            return false;
        }
        Result GetTransactionResult (string val)
        {
            Result result = Result.Valid;
            switch(val)
            {
                case "Valid":
                    result = Result.Valid;
                    break;
                case "Failure":
                    result = Result.Faliure;
                    break;
                case "Suspicious":
                    result = Result.Suspicious;
                    break;
                default:
                    result = Result.Faliure;
                    break;
            }
            return result;
        }
        CardType GetCardType(string val)
        {
            CardType cardTYpe = CardType.Debit;

            switch (val)
            {
                case "Credit":
                    cardTYpe = CardType.Credit;
                    break;
                case "Master":
                    cardTYpe = CardType.Master;
                    break;
                case "Visa":
                    cardTYpe = CardType.Visa;
                    break;
                default:
                    cardTYpe = CardType.Debit;
                    break;

            }
            return cardTYpe;
        }

        public IList<long> GetCardNumbers (int CustomerId)
        {
            SqlDataReader reader = null;
            try
            {
                SqlCommand sqlCommand = GetCommad($"SELECT CardNumber FROM cardInfo WHERE CardInfo.CustomerNo ={CustomerId}");
                reader =  sqlCommand.ExecuteReader();
                IList<long> cardNumbers = new List<long>();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        cardNumbers.Add(Convert.ToInt64(reader.GetString(0)));
                   
                }
                reader.Close();

                return cardNumbers;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Close();
            }
        }
    

       
    }
}

