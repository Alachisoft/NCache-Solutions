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
using System.Linq;
using System.Threading.Tasks;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using MatchHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreUpdateController : ControllerBase
    {
        /// <summary>
        /// cache handler
        /// </summary>
        private ICache _cache;

        /// <summary>
        /// Manages stored match updates
        /// </summary>
        private static StoreHandler _storeHandler;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configuration">configuration file appsettings.json</param>
        public ScoreUpdateController(IConfiguration configuration)
        {
            // get cache name from appsettings.json
            string cacheName = configuration.GetValue<string>("CacheName");

            // get read/write thru provider name from appsettings.json
            string readThruProviderName = configuration.GetValue<string>("ReadThruProviderName");
            string writeThruProviderName = configuration.GetValue<string>("WriteThruProviderName");

            // initialize store match handler if not initialized before
            if (_storeHandler == null)
                _storeHandler = new StoreHandler(cacheName, readThruProviderName, writeThruProviderName);

            // acquire cache handler
            _cache = CacheManager.GetCache(configuration.GetValue<string>("CacheName"));
        }

        /// <summary>
        /// POST api/scoreupdate/matchdetails
        /// get stored match updates
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <returns>list of match updates</returns>
        [Route("matchdetails")]
        [HttpPost]
        public ActionResult<List<MatchUpdate>> GetMatchDetails([FromForm] string matchName)
        {
            List<MatchUpdate> list = _storeHandler.FetchUpdates(matchName);
            return list;
        }

        /// <summary>
        /// POST api/scoreupdate/startmatch
        /// start match
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <param name="datetime">time of match</param>
        /// <returns>true or false</returns>
        [Route("startmatch")]
        [HttpPost]
        public ActionResult<bool> StartMatch([FromForm]string matchName, [FromForm]string datetime)
        {
            try
            {
                // check if match is not already created
                if (_isOngoingMatch(matchName))
                    return true;

                // create pub/sub topic on match start
                _cache.MessagingService.CreateTopic(matchName);

                // create start update
                MatchUpdate start = new MatchUpdate(datetime, "match_start", "stadium", "Match has Started");

                // send update for storing in cache
                _storeHandler.SaveUpdates(matchName, start);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// POST api/scoreupdate/stopmatch
        /// stop match
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <param name="datetime">time of match</param>
        /// <returns>true of false</returns>
        [Route("stopmatch")]
        [HttpPost]
        public ActionResult<bool> StopMatch([FromForm] string matchName, [FromForm] string datetime)
        {
            try
            {
                // create stop update
                MatchUpdate stop = new MatchUpdate(datetime, "match_end", "stadium", "Match has Stopped");

                // check if the match is already created
                if (!_isOngoingMatch(matchName))
                    return true;

                // create new Message packet with the match update
                Message message = new Message(stop);

                // fetch topic handler from cache
                ITopic topic = _cache.MessagingService.GetTopic(matchName);
                // publish Message on topic
                topic.Publish(message, DeliveryOption.All);

                // send update for storing in cache
                _storeHandler.SaveUpdates(matchName, stop);
                // clear all stored updates
                _storeHandler.ClearUpdates(matchName);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        [Route("updatematch")]
        [HttpPost]
        /// <summary>
        /// update match with any event
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <param name="datetime">time of match</param>
        /// <param name="type">type of event</param>
        /// <param name="entity">entity of event</param>
        /// <param name="data">data of event</param>
        /// <returns>true or false</returns>
        public ActionResult<bool> UpdateMatch([FromForm] string matchName, [FromForm] string datetime, [FromForm] string type, [FromForm] string entity, [FromForm] string data)
        {
            try
            {
                // send update to live match handler
                MatchUpdate matchUpdate = new MatchUpdate(datetime, type, entity, data);

                // check if the match is already created
                if (!_isOngoingMatch(matchName))
                    return false;

                // create new Message packet with the match update
                Message message = new Message(matchUpdate);

                // fetch topic handler from cache
                ITopic topic = _cache.MessagingService.GetTopic(matchName);
                // publish Message on topic
                topic.Publish(message, DeliveryOption.All);

                // store update to cache
                _storeHandler.SaveUpdates(matchName, new MatchUpdate(datetime, type, entity, data));
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// check if match is ongoing
        /// </summary>
        /// <param name="matchName">name of match</param>
        /// <returns>true or false</returns>
        private bool _isOngoingMatch(string matchName)
        {
            return _cache.MessagingService.GetTopic(matchName) != null;
        }
    }
}