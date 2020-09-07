using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Samples.Dapper.Models;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Alachisoft.NCache.Samples.Dapper.BackingSources
{
    public class WriteThruProvider : IWriteThruProvider
    {
        public SqlConnection Connection { get; set; }
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        public void Init(
                        IDictionary parameters,
                        string cacheId)
        {
            var connectionString = parameters["connectionString"] as string;
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public OperationResult WriteToDataSource(
                                        WriteOperation operation)
        {
            var operationResult =
                          new OperationResult(
                                      operation,
                                      OperationResult.Status.Failure);

            int rowsAffected = 0;

            Customer customer = null;

            if (operation.OperationType == WriteOperationType.Add ||
                operation.OperationType == WriteOperationType.Update)
            {
                customer = operation.ProviderItem.GetValue<Customer>();
            }

            if (operation.OperationType == WriteOperationType.Add)
            {
                var commandDefinition = new CommandDefinition(
                                                @"INSERT INTO dbo.Customers (
                                                        CustomerID, 
                                                        CompanyName, 
                                                        ContactName, 
                                                        ContactTitle, 
                                                        Address, 
                                                        City, 
                                                        Region, 
                                                        PostalCode, 
                                                        Country, 
                                                        Phone, 
                                                        Fax) 
                                                VALUES (
                                                        @CustomerID, 
                                                        @CompanyName, 
                                                        @ContactName, 
                                                        @ContactTitle, 
                                                        @Address, 
                                                        @City, 
                                                        @Region, 
                                                        @PostalCode, 
                                                        @Country, 
                                                        @Phone, 
                                                        @Fax);",
                                            customer,
                                            flags: CommandFlags.NoCache);

                rowsAffected = Connection.Execute(commandDefinition);
            }
            else if (operation.OperationType == WriteOperationType.Update)
            {
                var commandDefinition = new CommandDefinition(
                                              @"UPDATE dbo.Customers 
                                              SET    CompanyName = @CompanyName, 
                                                     ContactName = @ContactName, 
                                                     ContactTitle = @ContactTitle, 
                                                     Address = @Address, 
                                                     City = @City, 
                                                     Region = @Region, 
                                                     PostalCode = @PostalCode, 
                                                     Country = @Country, 
                                                     Phone = @Phone, 
                                                     Fax = @Fax
                                              WHERE CustomerID = @CustomerID;",
                                              customer,
                                              flags: CommandFlags.NoCache);

                rowsAffected = Connection.Execute(
                                            commandDefinition);
            }
            else if (operation.OperationType == WriteOperationType.Delete)
            {
                var customerId = 
                    operation.Key.Replace("Customer:CustomerID:", "").Trim();

                var commandDefinition = new CommandDefinition(
                                                "DELETE FROM dbo.Customers " +
                                                "WHERE CustomerID = @cId",
                                                new { cId = customerId },
                                                flags: CommandFlags.NoCache);

                rowsAffected = Connection.Execute(commandDefinition);
            }

            if (rowsAffected > 0)
            {
                operationResult.OperationStatus = OperationResult.Status.Success;
            }

            return operationResult;
        }

        public ICollection<OperationResult> WriteToDataSource(
                                                ICollection<WriteOperation> operations)
        {
            var operationResults = new List<OperationResult>();

            foreach (var operation in operations)
            {
                operationResults.Add(WriteToDataSource(operation));
            }

            return operationResults;
        }

        public ICollection<OperationResult> WriteToDataSource(
                            ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            throw new NotImplementedException();
        }
    }
}
