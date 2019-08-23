using Alachisoft.NCache.Client;
using Basket.FunctionalTests.Base;
using Microsoft.eShopOnContainers.Services.Basket.API.Infrastructure.Repositories;
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


using Microsoft.eShopOnContainers.Services.Basket.API.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Basket.FunctionalTests
{
    public class NCacheBasketRepositoryTests:BasketScenarioBase
    {
        [Fact]
        public async Task UpdateBasket_return_and_add_basket()
        {
            using (var server = CreateServer())
            {
                var ncache = server.Host.Services.GetRequiredService<ICache>();

                var ncacheBasketRepository = BuildBasketRepository(ncache);

                var basket = await ncacheBasketRepository.UpdateBasketAsync(new CustomerBasket("customerId")
                {
                    BuyerId = "buyerId",
                    Items = BuildBasketItems()
                });

                Assert.NotNull(basket);
                Assert.Single(basket.Items);
            }
        }

        [Fact]
        public async Task Delete_Basket_return_null()
        {

            using (var server = CreateServer())
            {
                var ncache = server.Host.Services.GetRequiredService<ICache>();

                var ncacheBasketRepository = BuildBasketRepository(ncache);

                var basket = await ncacheBasketRepository.UpdateBasketAsync(new CustomerBasket("customerId")
                {
                    BuyerId = "buyerId",
                    Items = BuildBasketItems()
                });

                var deleteResult = await ncacheBasketRepository.DeleteBasketAsync("buyerId");

                var result = await ncacheBasketRepository.GetBasketAsync(basket.BuyerId);

                Assert.True(deleteResult);
                Assert.Null(result);
            }
        }

        NCacheBasketRepository BuildBasketRepository(ICache cache)
        {
            var loggerFactory = new LoggerFactory();
            return new NCacheBasketRepository(loggerFactory, cache);
        }

        List<BasketItem> BuildBasketItems()
        {
            return new List<BasketItem>()
            {
                new BasketItem()
                {
                    Id = "basketId",
                    PictureUrl = "pictureurl",
                    ProductId = "productId",
                    ProductName = "productName",
                    Quantity = 1,
                    UnitPrice = 1
                }
            };
        }
    }
}
