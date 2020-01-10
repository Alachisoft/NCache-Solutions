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

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    // The NCacheChangeFeedObserver is used to process changes in the monitored collection
    // and invoke the DependencyChanged event on all affected instances of CosmosDbNotificationDependency
    [Serializable]
    internal class NCacheChangeFeedObserver : IChangeFeedObserver
    {
        // The _changeFeedProcessorKey field encapsulates information linking the dependent items in the cache
        // with the corresponding data in the database
        private readonly string _changeFeedProcessorKey;
        public NCacheChangeFeedObserver(
            string changeFeedProcessorKey)
        {
            _changeFeedProcessorKey = changeFeedProcessorKey;
        }
        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessChangesAsync(
            IChangeFeedObserverContext context,
            IReadOnlyList<Document> docs,
            CancellationToken cancellationToken)
        {
            var changedKeys = (from doc in docs select doc.Id).ToList();

            // Using the _changeFeedProcessorKey, we map the _id of the document to the keys using in the 
            // CosmosDbNotificationDependency **dependenciesOnKeys** static dictionary
            for (int i = 0; i < changedKeys.Count; i++)
            {
                changedKeys[i] = $"{_changeFeedProcessorKey} {changedKeys[i]}";
            }

            var markedDependencyKeys = new List<string>();
            HashSet<CosmosDbNotificationDependency> markedDependencies;

            foreach (var key in changedKeys)
            {
                // Only if the changed document has a dependent item in the cache do we invoke the associated dependency
                if (CosmosDbNotificationDependency.dependenciesOnKeys.ContainsKey(key))
                {
                    markedDependencies = new HashSet<CosmosDbNotificationDependency>(CosmosDbNotificationDependency.dependenciesOnKeys[key]);

                    foreach (var dep in markedDependencies)
                    {
                        dep.DependencyChanged.Invoke(this);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
