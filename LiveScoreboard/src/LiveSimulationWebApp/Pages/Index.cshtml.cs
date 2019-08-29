//Copyright(c) 2019 Alachisoft

//Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0


//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using LiveScoreLeaderboard.SignalR;
using MatchHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace LiveScoreLeaderboard.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Dependency injection for acquiring SignalR hub control
        /// </summary>
        private static IHubContext<SignalRHub> _hub;

        /// <summary>
        /// cache handler
        /// </summary>
        private ICache _cache;

        /// <summary>
        /// Manages stored match updates
        /// </summary>
        private static StoreHandler _storeHandler;

        private static ITopicSubscription _matchSubscription;

        /// <summary>
        /// constructor for <see cref="IndexModel"/>
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="configuration"></param>
        public IndexModel(IHubContext<SignalRHub> hub, IConfiguration configuration)
        {
            // store SignalR hub reference for use
            _hub = hub;

            // get cache name from appsettings.json
            string cacheName = configuration.GetValue<string>("CacheName");

            // get read/write thru provider name from appsettings.json
            string readThruProviderName = configuration.GetValue<string>("ReadThruProviderName");
            string writeThruProviderName = configuration.GetValue<string>("WriteThruProviderName");

            // acquire cache handler
            _cache = CacheManager.GetCache(cacheName);

            // initialize store match handler if not initialized before
            if (_storeHandler == null)
                _storeHandler = new StoreHandler(cacheName, readThruProviderName, writeThruProviderName);
        }

        /// <summary>
        /// start pub/sub and fetch stored match details from cache
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <returns>list of updates</returns>
        public JsonResult OnPostMatchDetails(string matchName)
        {
            // subscribe this server to ongoing match topic
            if(_matchSubscription == null)
            {
                ITopic topic = _cache.MessagingService.GetTopic(matchName);
                // if topic does not exist, return false
                if(topic == null)
                {
                    return new JsonResult(false);
                }
                _matchSubscription = topic.CreateSubscription(MatchUpdates);
            }

            // fetch details of ongoing matches
            List<MatchUpdate> list = _storeHandler.FetchUpdates(matchName);
            return new JsonResult(list);
        }

        /// <summary>
        /// stop match
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <returns></returns>
        public JsonResult OnPostStopMatch(string matchName)
        {
            try
            {
                _matchSubscription.UnSubscribe();
                _matchSubscription = null;
                _cache.MessagingService.DeleteTopic(matchName);
            }
            catch (Exception e)
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        /// <summary>
        /// event listener for match updates
        /// </summary>
        /// <param name="matchUpdate"></param>
        public void MatchUpdates(object sender, MessageEventArgs args)
        {
            _hub.Clients.All.SendAsync("MatchUpdate", new JsonResult((MatchUpdate)args.Message.Payload));
        }
    }
}
