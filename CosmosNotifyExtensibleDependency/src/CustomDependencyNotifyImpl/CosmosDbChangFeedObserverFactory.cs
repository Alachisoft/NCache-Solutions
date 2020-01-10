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

using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using System;

namespace Alachisoft.NCache.Samples
{
    // The change feed observer factory that will be used to create the change feed observers
    [Serializable]
    internal class CosmosDbChangFeedObserverFactory : IChangeFeedObserverFactory
    {
        // The _changeFeedProcessor key field which maps the _id field of the documents in the database to the keys used in the 
        // CosmosDbNotificationDependency **dependenciesOnKeys** static dictionary
        private readonly string _changeFeedProcessorKey;

        public CosmosDbChangFeedObserverFactory(
            string changeFeedProcessorKey)
        {
            _changeFeedProcessorKey = changeFeedProcessorKey;
        }
        public IChangeFeedObserver CreateObserver()
        {
            return new NCacheChangeFeedObserver(_changeFeedProcessorKey);
        }
    }
}
