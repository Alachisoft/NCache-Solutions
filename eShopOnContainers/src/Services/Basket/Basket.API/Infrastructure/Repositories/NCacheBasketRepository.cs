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


using Alachisoft.NCache.Client;
using Microsoft.eShopOnContainers.Services.Basket.API.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Basket.API.Infrastructure.Repositories
{
    public class NCacheBasketRepository : IBasketRepository
    {
        private readonly ILogger<NCacheBasketRepository> _logger;
        private ICache _cache;

        public NCacheBasketRepository(ILoggerFactory loggerFactory, ICache cache)
        {
            _logger = loggerFactory.CreateLogger<NCacheBasketRepository>();
            _cache = cache;
        }


        public async Task<bool> DeleteBasketAsync(string id)
        {
            _logger.LogTrace($"Removing basket with ID {id}");
            //_cache.Remove(id);
            await Task.Run(() => _cache.Remove(id));
            _logger.LogInformation($"Basket with ID {id} succesfully removed");
            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await Task.Run(() => _cache.Get<string>(customerId));

            if (string.IsNullOrEmpty(data))
                return null;

            var result = JsonConvert.DeserializeObject<CustomerBasket>(data);

            return result;
        }

        public IEnumerable<string> GetUsers()
        {
            var enumerator = _cache.GetEnumerator();
            List<string> keys = new List<string>();

            while (enumerator.MoveNext())
            {
                keys.Add(((DictionaryEntry)enumerator.Current).Key as string);
            }

            return keys;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var result = await _cache.InsertAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            _logger.LogInformation("Basket item persisted succesfully.");
            return await GetBasketAsync(basket.BuyerId);
        }
    }
}
