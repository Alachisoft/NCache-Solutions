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
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    // this message is sent back to transaction manager after transaction completion
   public class TransactionCompletedMessage 
    {
        BaseManager transactionManager;
        public TransactionCompletedMessage (BaseManager mgr)
        {
            transactionManager = mgr;
        }
        public void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            if (args.TopicName.Equals(Topics.REPLIESTOPICS, StringComparison.InvariantCultureIgnoreCase))
            {
                if (args != null && args.Message != null)
                {
                    transactionManager.OnRequest(args);
                }
            }
        }
    }
}
