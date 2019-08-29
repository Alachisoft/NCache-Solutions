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
using System.Collections;
using System.Diagnostics;

namespace Alachisoft.NCache.PersistenceProvider
{
    /// <summary>
    /// This class contains the implementation for Init and Dispose method is Persistence Providers
    /// </summary>
    public class ProviderBase
    {
        private string _connectionString;
        private IPersistenceProvider _persistenceProvider = null;
        private IProviderLogger _logger = new EventViewerLogging();

        /// <summary>
        /// PersistenceProvider to used persistent store
        /// </summary>
        public IPersistenceProvider PersistenceProvider
        {
            get
            {
                return _persistenceProvider;
            }
            set
            {
                _persistenceProvider = value;
            }
        }
        /// <summary>
        /// Logger used underneath for logging
        /// </summary>
        public IProviderLogger Logger
        {
            get
            {
                return _logger;
            }
            set
            {
                _logger = value;
            }
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
                if (parameters == null) throw new ArgumentNullException(nameof(parameters));
                if (parameters.Contains("connectionstring"))
                {
                    _connectionString = (string)parameters["connectionstring"];
                }
                //Connection string for cosmos db must contain a pattren
                // ~ separated serviceEndPoint~authKey~databaseName                
                if (parameters.Contains("FQN"))
                {
                    PersistenceProvider= ReflectionUtil.CreateInstanceWithReflection((string)parameters["FQN"],cacheId);
                }
                else
                {
                    throw new ArgumentException("FQN");
                }
                PersistenceProvider.Init(_connectionString);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
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
                PersistenceProvider.Dispose();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                throw ex;
            }
        }

    }
}
