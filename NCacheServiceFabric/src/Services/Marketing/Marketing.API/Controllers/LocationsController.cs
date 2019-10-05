namespace Microsoft.eShopOnContainers.Services.Marketing.API.Controllers
{
    using Alachisoft.NCache.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.eShopOnContainers.Services.Marketing.API.Dto;
    using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure;
    using Microsoft.eShopOnContainers.Services.Marketing.API.Model;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    [Authorize]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly MarketingContext _context;
        private readonly MarketingSettings _settings;

        public LocationsController(MarketingContext context, IOptionsSnapshot<MarketingSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }

        [HttpGet]
        [Route("api/v1/campaigns/{campaignId:int}/locations/{userLocationRuleId:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserLocationRuleDTO),(int)HttpStatusCode.OK)]
        public ActionResult<UserLocationRuleDTO> GetLocationByCampaignAndLocationRuleId(int campaignId, 
            int userLocationRuleId)
        {
            if (campaignId < 1 || userLocationRuleId < 1)
            {
                return BadRequest();
            }

            UserLocationRule location = null;
            if (!_settings.CachingEnabled)
            {
                location = _context.Rules
                        .OfType<UserLocationRule>()
                        .SingleOrDefault(c => c.CampaignId == campaignId && c.Id == userLocationRuleId); 
            }
            else
            {
                var options = new CachingOptions();
                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                location = _context.Rules
                    .OfType<UserLocationRule>()
                    .DeferredSingleOrDefault(c => c.CampaignId == campaignId && c.Id == userLocationRuleId)
                    .FromCache(options);
            }

            if (location is null)
            {
                return NotFound();
            }

            return MapUserLocationRuleModelToDto(location);
        }

        [HttpGet]
        [Route("api/v1/campaigns/{campaignId:int}/locations")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<UserLocationRuleDTO>), (int)HttpStatusCode.OK)]
        public ActionResult<List<UserLocationRuleDTO>> GetAllLocationsByCampaignId(int campaignId)
        {
            if (campaignId < 1)
            {
                return BadRequest();
            }

            List<UserLocationRule> locationList = null;
            if (!_settings.CachingEnabled)
            {
                locationList = _context.Rules
                        .OfType<UserLocationRule>()
                        .Where(c => c.CampaignId == campaignId)
                        .ToList(); 
            }
            else
            {
                var options = new CachingOptions
                {
                    StoreAs = StoreAs.SeperateEntities
                };
                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                locationList = _context.Rules
                    .OfType<UserLocationRule>()
                    .Where(c => c.CampaignId == campaignId)
                    .FromCache(options)
                    .ToList();
            }

            if(locationList is null)
            {
                return Ok();
            }

            return MapUserLocationRuleModelListToDtoList(locationList);
        }

        [HttpPost]
        [Route("api/v1/campaigns/{campaignId:int}/locations")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateLocationAsync(int campaignId, [FromBody] UserLocationRuleDTO locationRuleDto)
        {
            if (campaignId < 1 || locationRuleDto is null)
            {
                return BadRequest();
            }

            var locationRule = MapUserLocationRuleDtoToModel(locationRuleDto);
            locationRule.CampaignId = campaignId;

            await _context.Rules.AddAsync(locationRule);
            await _context.SaveChangesAsync();

            if (_settings.CachingEnabled)
            {
                var cache = _context.GetCache();

                var options = new CachingOptions();
                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));
                await Task.Run(() => cache.Insert(locationRule, out string cacheKey, options));
            }

            return CreatedAtAction(nameof(GetLocationByCampaignAndLocationRuleId),
                new { campaignId = campaignId, userLocationRuleId = locationRule.Id }, null);
        }

        [HttpDelete]
        [Route("api/v1/campaigns/{campaignId:int}/locations/{userLocationRuleId:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteLocationByIdAsync(int campaignId, int userLocationRuleId)
        {
            if (campaignId < 1 || userLocationRuleId < 1)
            {
                return BadRequest();
            }

            UserLocationRule locationToDelete = null;
            if (!_settings.CachingEnabled)
            {
                locationToDelete = _context.Rules
                        .OfType<UserLocationRule>()
                        .SingleOrDefault(c => c.CampaignId == campaignId && c.Id == userLocationRuleId); 
            }
            else
            {
                var options = new CachingOptions();
                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                locationToDelete = await _context.Rules
                        .OfType<UserLocationRule>()
                        .DeferredSingleOrDefault(c => c.CampaignId == campaignId && c.Id == userLocationRuleId)
                        .FromCacheAsync(options);
            }

            if (locationToDelete is null)
            {
                return NotFound();
            }

            _context.Rules.Remove(locationToDelete);
            await _context.SaveChangesAsync();

            if (_settings.CachingEnabled)
            {
                var cache = _context.GetCache();
                await Task.Run(() => cache.Remove(locationToDelete));
            }

            return NoContent();
        }

        private List<UserLocationRuleDTO> MapUserLocationRuleModelListToDtoList(List<UserLocationRule> userLocationRuleList)
        {
            var userLocationRuleDtoList = new List<UserLocationRuleDTO>();

            userLocationRuleList.ForEach(userLocationRule => userLocationRuleDtoList
                .Add(MapUserLocationRuleModelToDto(userLocationRule)));

            return userLocationRuleDtoList;
        }

        private UserLocationRuleDTO MapUserLocationRuleModelToDto(UserLocationRule userLocationRule)
        {
            return new UserLocationRuleDTO
            {
                Id = userLocationRule.Id,
                Description = userLocationRule.Description,
                LocationId = userLocationRule.LocationId
            };
        }

        private UserLocationRule MapUserLocationRuleDtoToModel(UserLocationRuleDTO userLocationRuleDto)
        {
            return new UserLocationRule
            {
                Id = userLocationRuleDto.Id,
                Description = userLocationRuleDto.Description,
                LocationId = userLocationRuleDto.LocationId
            };
        }
    }
}