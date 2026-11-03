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

using Microsoft.Extensions.Configuration;
using Models;
using MongoDB.Driver;
using MongoDbNotificationDependencyNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbNotifyExtensibleDependencyImpl
{
    // This class listens to the change stream and trigger, register, and unregister dependencies 

    [Serializable]
    public static class MongoDbChangeStreamListner
    {
        private static Dictionary<string, MongoDbNotificationDependency> _dependencyItems;  // Contains items having dependency
        private static Queue<string> _triggerDependencyItemsQueue;  // Contains items for which dependency is to be triggered
        private static ManualResetEvent _manualResetEvent;          // Task awaiter

        static MongoDbChangeStreamListner()
        {
            _dependencyItems = new Dictionary<string, MongoDbNotificationDependency>();

            _triggerDependencyItemsQueue = new Queue<string>();

            _manualResetEvent = new ManualResetEvent(false);

            Task changeStreamListnerTask = new Task(() => WatchChangeStream());     // This task listens to the MongoDB change stream
            changeStreamListnerTask.Start();

            Task triggerDependencyTask = new Task(() => TriggerDependency());       // This task triggers dependencies
            triggerDependencyTask.Start();
        }

        // Registers cache items having dependency on database items
        public static void RegisterDependency(string cacheKey, MongoDbNotificationDependency dependencyObject)
        {
            lock (_dependencyItems)
            {
                _dependencyItems.Add(cacheKey, dependencyObject);
            }
        }

        // Unregisters cache items having dependency on database items
        public static void UnregisterDependency(string cacheKey)
        {
            lock (_dependencyItems)
            {
                _dependencyItems.Remove(cacheKey);
            }
        }

        // Listening to MongoDB change stream and marks items to be removed when data in database updates.
        private static void WatchChangeStream()
        {
            // Filtering the change stream to get only Update events.
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<Customer>>()
                .Match(c => c.OperationType == ChangeStreamOperationType.Update);

            // Asking change stream to return full document on update event.
            var changeStreamOptions = new ChangeStreamOptions()
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            // Starting listening to change stream
            using (var cursor = MongoDbNotificationDependency._coll.Watch(pipeline, changeStreamOptions))
            {
                foreach (var change in cursor.ToEnumerable())
                {
                    string key = "Customer:" + change.FullDocument.CustomerId;
                    if (_dependencyItems.ContainsKey(key))
                    {
                        lock (_triggerDependencyItemsQueue)
                        {
                            _triggerDependencyItemsQueue.Enqueue(key);          // Adding item in Queue to trigger dependency

                            _manualResetEvent.Set();                            // Resuming the dependency trigger task
                        }
                    }
                }
            }
        }

        // Triggers dependency agains stale items in cache
        private static void TriggerDependency()
        {
            while (true)
            {
                try
                {
                    _manualResetEvent.WaitOne();                            // Pausing the task to save resources

                    string key;
                    lock (_triggerDependencyItemsQueue)
                    {
                        key = _triggerDependencyItemsQueue.Dequeue();
                    }
                    lock (_dependencyItems)
                    {
                        _dependencyItems[key].DependencyChanged.Invoke(_dependencyItems[key]);

                        UnregisterDependency(key);                          // Removing item from dependent items dictionary
                    }
                }
                catch (Exception)
                {
                    _manualResetEvent.Reset();
                    continue;
                }
            }
        }
    }
}
