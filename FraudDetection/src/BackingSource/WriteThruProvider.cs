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
using Alachisoft.NCache.Runtime.DatasourceProviders;
using CustomerSample;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BackingSource
{
    public class SqlWriteThruProvider : IWriteThruProvider
    {
        /// <summary>
        /// Object used to communicate with the Datasource.
        /// </summary>
        private SqlDataSource sqlDatasource;

        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">Startup paramters defined in the configuration</param>
        /// <param name="cacheId">Define for which cache provider is configured</param>
        public void Init(IDictionary parameters, string cacheId)
        {
            object connString = parameters["connstring"];
            sqlDatasource = new SqlDataSource();
            sqlDatasource.Connect(connString == null ? "" : connString.ToString());
        }

        /// <summary>
        /// Perform tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public void Dispose()
        {
            sqlDatasource.DisConnect();
        }

        #region IWriteThruProvider Members

        /// <summary>
        /// Write multiple cache operations to data source
        /// </summary>
        /// <param name="operations">Collection of <see cref="WriteOperation"/></param>
        /// <returns>Collection of <see cref="OperationResult"/> to cache</returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<WriteOperation> operations)
        {
            // initialize collection of results to return to cache
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // iterate over each operation sent by cache
            foreach (var item in operations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(item, OperationResult.Status.Failure);
                // get object from provider cache item
                FraudRequest value = item.ProviderItem.GetValue<FraudRequest>();
                // check if the type is what you need
                if (value.GetType().Equals(typeof(FraudRequest)))
                {
                    // initialize variable for confirmation of write operation
                    // perform write command and get confirmation from data source
                    bool result = sqlDatasource.SaveTransaction(value);
                    // if write operation is successful, change status of operationResult
                    if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                }
                // insert result operation to collect of results
                operationResults.Add(operationResult);
            }

            // send result to cache
            return operationResults;

        }

        /// <summary>
        /// Write multiple data type cache operations to data source
        /// </summary>
        /// <param name="dataTypeWriteOperations">Collection of <see cref="DataTypeWriteOperation"/></param>
        /// <returns>Collection of <see cref="OperationResult"/> to cache</returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            // initialize collection of results to return to cache
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // initialize variable for confirmation of write operation
            bool result = false;
            // iterate over each operation sent by cache
            foreach (DataTypeWriteOperation operation in dataTypeWriteOperations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
                // determine the type of data structure
                if (operation.DataType == DistributedDataType.List)
                {
                        if (operation.OperationType.Equals(DatastructureOperationType.UpdateDataType) || (operation.OperationType.Equals(DatastructureOperationType.AddToDataType)))
                            result = sqlDatasource.SaveTransaction((FraudRequest)operation.ProviderItem.Data);
                        if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                     
                }
                // add result to list of operation results
                operationResults.Add(operationResult);
            }
            // return list of operations to cache
            return operationResults;
        }

        /// <summary>
        /// write an operation to data source
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public OperationResult WriteToDataSource(WriteOperation operation)
        {
            // initialize variable for confirmation of write operation
            bool result = false;
            // initialize operation result with failure
            OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
            // get value of object
            FraudRequest value = operation.ProviderItem.GetValue<FraudRequest>();
            // check if value is the type you need
            if (value.GetType().Equals(typeof(FraudRequest)))
            {
                // send data to cache for writing
                result = sqlDatasource.SaveTransaction((FraudRequest)value);
                // if write operatio is success, change status of operation result
                if (result) operationResult.OperationStatus = OperationResult.Status.Success;
            }
            // return result to cache
            return operationResult;
        }

        #endregion
    }
}

