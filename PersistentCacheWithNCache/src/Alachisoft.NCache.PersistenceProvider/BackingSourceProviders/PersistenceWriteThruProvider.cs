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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alachisoft.NCache.PersistenceProvider
{
    /// <summary>
    /// PersistenceWriteThruProvider implementation which use IPersistenceStore
    /// to Store items from cache to persisted store
    /// </summary>
    public class PersistenceWriteThruProvider : ProviderBase, IWriteThruProvider
    {
        /// <summary>
        /// Write to data source
        /// </summary>
        /// <param name="operation">contians the operation to be written</param>
        /// <returns></returns>
        public OperationResult WriteToDataSource(WriteOperation operation)
        {
            if(operation==null)
            {
                throw new ArgumentNullException(nameof(operation));
            }
            OperationResult operationResults = new OperationResult(operation, OperationResult.Status.Success);
            try
            {
                switch (operation.OperationType)
                {
                    case WriteOperationType.Add:
                        var items = new Dictionary<string, ProviderItemBase>();
                        items.Add(operation.Key, operation.ProviderItem);
                        PersistenceProvider.Add(items);
                        break;

                    case WriteOperationType.Update:
                        var itemsToUpdate = new Dictionary<string, ProviderItemBase>();
                        itemsToUpdate.Add(operation.Key, operation.ProviderItem);
                        PersistenceProvider.Insert(itemsToUpdate);
                        break;

                    case WriteOperationType.Delete:
                        PersistenceProvider.Remove(new string[] { operation.Key });
                        break;

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                operationResults = new OperationResult(operation, OperationResult.Status.Failure);
            }
            return operationResults;
        }
        /// <summary>
        /// Write to data source , contians multiple operations
        /// </summary>
        /// <param name="operations">collection of operations</param>
        /// <returns></returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<WriteOperation> operations)
        {
            ICollection<OperationResult> opResults = new List<OperationResult>();
            foreach (var operation in operations)
            {
                opResults.Add(WriteToDataSource(operation));
            }
            return opResults;
        }
        /// <summary>
        /// Write to datasource overload with datatype operations
        /// </summary>
        /// <param name="dataTypeWriteOperations">collection of datatype operations</param>
        /// <returns></returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            //Implementation is required in order to use data structures
            ICollection<OperationResult> dtOperationResults = new List<OperationResult>();
            foreach (var dtOperation in dataTypeWriteOperations)
            {
                OperationResult opResult = new OperationResult(dtOperation, OperationResult.Status.Success, "");
                try
                {
                    switch (dtOperation.OperationType)
                    {

                        case DatastructureOperationType.CreateDataType:
                            IDictionary<string, ProviderItemBase> dict = new Dictionary<string, ProviderItemBase>();
                            dict.Add(dtOperation.Key, dtOperation.ProviderItem);
                            PersistenceProvider.Insert(dict);
                            break;

                        case DatastructureOperationType.DeleteDataType:
                            PersistenceProvider.Remove(new string[] { dtOperation.Key });
                            break;

                        case DatastructureOperationType.AddToDataType:
                            
                            PersistenceProvider.AddToDataType(dtOperation.Key, dtOperation.ProviderItem, dtOperation.DataType);
                            break;

                        case DatastructureOperationType.UpdateDataType:
                            PersistenceProvider.UpdateToDataType(dtOperation.Key, dtOperation.ProviderItem, dtOperation.DataType);
                            break;

                        case DatastructureOperationType.DeleteFromDataType:
                            PersistenceProvider.RemoveFromDataType(dtOperation.Key, dtOperation.DataType);
                            break;
                    } 

                }
                catch (Exception ex)
                {
                    opResult.Exception = ex;
                    opResult.OperationStatus = OperationResult.Status.Failure;
                }
                dtOperationResults.Add(opResult);
            }

            return dtOperationResults;
        }
    }
}
