using Alachisoft.NCache.EntityFrameworkCore;
using Catalog.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Catalog.API.Model;
using Microsoft.eShopOnContainers.Services.Catalog.API.ViewModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

        public CatalogController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings, ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIdsAsync(ids);

                if (!items.Any())
                {
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");
                }

                return Ok(items);
            }

            var totalItems = -1L;
            var itemsOnPage = new List<CatalogItem>();

            if (!_settings.EFCoreCachingEnabled)
            {
                totalItems = await _catalogContext.CatalogItems
                .LongCountAsync();

                itemsOnPage = await _catalogContext.CatalogItems
                    .OrderBy(c => c.Name)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                var options = new CachingOptions
                {
                    //   QueryIdentifier = $"ItemsAsyncCount",
                    //   StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                totalItems = await _catalogContext.CatalogItems.DeferredLongCount().FromCacheAsync(options);

                // options.QueryIdentifier = $"ItemsAsync-{pageSize}-{pageIndex}";
                options.StoreAs = StoreAs.SeperateEntities;

                itemsOnPage = (await _catalogContext.CatalogItems
                    .OrderBy(c => c.Name)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .FromCacheAsync(options))
                    .ToList();
            }


            /* The "awesome" fix for testing Devspaces */

            /*
            foreach (var pr in itemsOnPage) {
                pr.Name = "Awesome " + pr.Name;
            }

            */

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);

        }

        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<CatalogItem>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = new List<CatalogItem>();
            if (_settings.EFCoreCachingEnabled)
            {
                var options = new CachingOptions
                {
                    //   QueryIdentifier = $"GetItemsByIdsAsync-{ids}",
                    StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                items = (await _catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).FromCacheAsync(options)).ToList();
            }
            else
            {
                items = await _catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
            }


            items = ChangeUriPlaceholder(items);

            return items;

        }


        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            CatalogItem item = null;

            if (!_settings.EFCoreCachingEnabled)
            {
                item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == id);
            }
            else
            {
                var options = new CachingOptions
                {
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                item = await _catalogContext.CatalogItems.DeferredSingleOrDefault(ci => ci.Id == id).FromCacheAsync(options);
            }

            var baseUri = _settings.PicBaseUrl;

            item.FillProductUrl(baseUri);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsWithNameAsync(string name, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            long totalItems = -1L;
            List<CatalogItem> itemsOnPage = new List<CatalogItem>();

            if (!_settings.EFCoreCachingEnabled)
            {
                totalItems = await _catalogContext.CatalogItems
                        .Where(c => c.Name.StartsWith(name))
                        .LongCountAsync();

                itemsOnPage = await _catalogContext.CatalogItems
                    .Where(c => c.Name.StartsWith(name))
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                var options = new CachingOptions
                {
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                totalItems = await _catalogContext.CatalogItems
                          .Where(c => c.Name.StartsWith(name))
                          .DeferredLongCount()
                          .FromCacheAsync(options);

                // options.QueryIdentifier = $"ItemsWithNameAsync-{name}-{pageSize}-{pageIndex}";

                options.StoreAs = StoreAs.SeperateEntities;

                itemsOnPage = (await _catalogContext.CatalogItems
                    .Where(c => c.Name.StartsWith(name))
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .FromCacheAsync(options))
                    .ToList();
            }

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandIdAsync(int catalogTypeId, int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            root = root.Where(ci => ci.CatalogTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);
            }

            long totalItems = -1L;
            var itemsOnPage = new List<CatalogItem>();

            if (!_settings.EFCoreCachingEnabled)
            {
                totalItems = await root
                    .LongCountAsync();

                itemsOnPage = await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                var options = new CachingOptions();

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                totalItems = await root
                    .DeferredLongCount()
                    .FromCacheAsync(options);


                options.StoreAs = StoreAs.SeperateEntities;

                itemsOnPage = (await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .FromCacheAsync(options)).ToList();

            }

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/all/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByBrandIdAsync(int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);
            }

            long totalItems = -1L;
            var itemsOnPage = new List<CatalogItem>();

            if (!_settings.EFCoreCachingEnabled)
            {
                totalItems = await root
                        .LongCountAsync();

                itemsOnPage = await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();
                
            }
            else
            {
                var options = new CachingOptions();

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                totalItems = await root
                    .DeferredLongCount()
                    .FromCacheAsync(options);

                options.StoreAs = StoreAs.SeperateEntities;

                itemsOnPage = (await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .FromCacheAsync(options)).ToList();
            }

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/CatalogTypes
        [HttpGet]
        [Route("catalogtypes")]
        [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogType>>> CatalogTypesAsync()
        {
            if (!_settings.EFCoreCachingEnabled)
            {
                return await _catalogContext.CatalogTypes.ToListAsync();
            }
            else
            {
                var options = new CachingOptions
                {
                    StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                var result = await _catalogContext.CatalogTypes.FromCacheAsync(options);

                return result.ToList();
            }
        }

        // GET api/v1/[controller]/CatalogBrands
        [HttpGet]
        [Route("catalogbrands")]
        [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogBrand>>> CatalogBrandsAsync()
        {
            if (!_settings.EFCoreCachingEnabled)
            {
                return await _catalogContext.CatalogBrands.ToListAsync();
            }
            else
            {
                var options = new CachingOptions
                {
                    StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                var result = await _catalogContext.CatalogBrands.FromCacheAsync(options);

                return result.ToList();
            }
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody]CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);

            if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

                // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await _catalogContext.SaveChangesAsync();
            }

            var cache = _catalogContext.GetCache();

            await Task.Run(() =>
            {
                cache.Insert(catalogItem, out string cacheKey, new CachingOptions
                {
                });
            });

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody]CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _catalogContext.CatalogItems.Add(item);

            await _catalogContext.SaveChangesAsync();

            var cache = _catalogContext.GetCache();

            await Task.Run(() =>
            {
                cache.Insert(item, out string cacheKey, new CachingOptions
                {
                });
            });

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = _catalogContext.CatalogItems.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _catalogContext.CatalogItems.Remove(product);

            await _catalogContext.SaveChangesAsync();

            var cache = _catalogContext.GetCache();

            cache.Remove(product);

            return NoContent();
        }

        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = _settings.PicBaseUrl;

            foreach (var item in items)
            {
                item.FillProductUrl(baseUri);
            }

            return items;
        }
    }
}
