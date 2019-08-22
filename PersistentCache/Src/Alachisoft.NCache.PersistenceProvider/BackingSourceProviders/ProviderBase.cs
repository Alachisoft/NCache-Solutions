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

using PersistenceProviders;
using PersistentNCache.Logging;
using System;
using System.Collections;
using System.Diagnostics;

namespace PersistentNCache.BackingSourceProviders
{
    /// <summary>
    /// This class contains the implementation for Init and Dispose method is Persistence Providers
    /// </summary>
    public class ProviderBase
    {
        private string _connectionString;
        protected IPersistenceProvider _persistenceProvider = null;
        protected IProviderLogger _logger = new EventViewerLogging();

        /// <summary>
        /// CreateInstance for IPersistenceProvider implementation
        /// </summary>
        /// <param name="fqn"> fully qualified name for class</param>
        /// <param name="cacheName">Name of the Cache</param>
        /// <returns></returns>
        private IPersistenceProvider CreatePersistenceProviderInstance(string fqn,string cacheName)
        {
            return Util.CreateInstanceWithReflection(fqn, cacheName);
        }
       
        /// <summary>
        /// Initilization of database
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cacheId"></param>
        public virtual void Init(IDictionary parameters, string cacheId)
        {
            try
            {
                if (parameters == null) throw new ArgumentNullException(parameters.ToString());
                if (parameters.Contains("connectionstring"))
                {
                    _connectionString = (string)parameters["connectionstring"];
                }
                //Connection string for cosmos db must contain a pattren
                // ~ separated serviceEndPoint~authKey~databaseName                
                if (parameters.Contains("FQN"))
                {
                    _persistenceProvider= Util.CreateInstanceWithReflection((string)parameters["FQN"],cacheId);
                }
                else
                {
                    throw new ArgumentNullException("FQN");
                }
                _persistenceProvider.Init(_connectionString);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        ///  Dispose for the Provider
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                _persistenceProvider.Dispose();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
