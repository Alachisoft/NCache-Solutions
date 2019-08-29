//  Copyright (c) 2018 Alachisoft
//  
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//     http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License

using System;
using System.Collections.Generic;
using System.Text;

namespace MatchHandler
{
    [Serializable]
    public class MatchUpdate
    {
        /// <summary>
        /// time at which the update occurred
        /// </summary>
        public string Datetime { get; set; }
        /// <summary>
        /// defines the type this update belongs to
        /// </summary>
        public string UpdateType { get; private set; }
        
        /// <summary>
        /// defines the entity this update belongs to
        /// </summary>
        public string Entity { get; private set; }

        /// <summary>
        /// any additional data to be provided with the update
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="datetime">time at which the event occurred</param>
        /// <param name="updateType">type of update</param>
        /// <param name="entity">entity of update</param>
        /// <param name="data">additional data with the update</param>
        public MatchUpdate(string datetime, string updateType, string entity, object data = null)
        {
            Datetime = datetime;
            UpdateType = updateType;
            Entity = entity;
            Data = data;
        }
    }
}
