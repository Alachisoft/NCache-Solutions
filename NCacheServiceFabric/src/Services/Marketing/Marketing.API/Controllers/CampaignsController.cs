using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure.Services;

namespace Microsoft.eShopOnContainers.Services.Marketing.API.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Infrastructure.Repositories;
    using AspNetCore.Mvc;
    using Infrastructure;
    using System.Threading.Tasks;
    using Model;
    using EntityFrameworkCore;
    using Dto;
    using AspNetCore.Authorization;
    using Extensions.Options;
    using Microsoft.eShopOnContainers.Services.Marketing.API.ViewModel;
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using Alachisoft.NCache.EntityFrameworkCore;

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly MarketingContext _context;
        private readonly MarketingSettings _settings;
        private readonly IMarketingDataRepository _marketingDataRepository;
        private readonly IIdentityService _identityService;

        public CampaignsController(MarketingContext context,
            IMarketingDataRepository marketingDataRepository,
             IOptionsSnapshot<MarketingSettings> settings,
            IIdentityService identityService)
        {
            _context = context;
            _marketingDataRepository = marketingDataRepository;
            _settings = settings.Value;
            _identityService = identityService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CampaignDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CampaignDTO>>> GetAllCampaignsAsync()
        {
            var campaignList = new List<Campaign>();
            if (!_settings.CachingEnabled)
            {
                campaignList = await _context.Campaigns.ToListAsync(); 
            }
            else
            {
                var options = new CachingOptions
                {
                    StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                campaignList = (await _context.Campaigns.FromCacheAsync(options)).ToList();
            }

            if (campaignList is null)
            {
                return Ok();
            }

            return MapCampaignModelListToDtoList(campaignList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CampaignDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<CampaignDTO>> GetCampaignByIdAsync(int id)
        {
            Campaign campaign = null;

            if (!_settings.CachingEnabled)
            {
                campaign = await _context.Campaigns.SingleOrDefaultAsync(c => c.Id == id); 
            }
            else
            {
                var options = new CachingOptions();

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                campaign = await _context.Campaigns.DeferredSingleOrDefault(c => c.Id == id).FromCacheAsync(options);
            }

            if (campaign is null)
            {
                return NotFound();
            }

            return MapCampaignModelToDto(campaign);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateCampaignAsync([FromBody] CampaignDTO campaignDto)
        {
            if (campaignDto is null)
            {
                return BadRequest();
            }

            var campaign = MapCampaignDtoToModel(campaignDto);

            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();

            if (_settings.CachingEnabled)
            {
                var cache = _context.GetCache();
                var cachingOptions = new CachingOptions();
                cachingOptions.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));
                await Task.Run(() => cache.Insert(campaign, out string cacheKey, cachingOptions));
            }

            return CreatedAtAction(nameof(GetCampaignByIdAsync), new { id = campaign.Id }, null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateCampaignAsync(int id, [FromBody] CampaignDTO campaignDto)
        {
            if (id < 1 || campaignDto is null)
            {
                return BadRequest();
            }

            Campaign campaignToUpdate = null;

            if (!_settings.CachingEnabled)
            {
                campaignToUpdate = await _context.Campaigns.FindAsync(id); 
            }
            else
            {
                var options = new CachingOptions
                {
                    StoreAs = StoreAs.SeperateEntities
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));

                campaignToUpdate = await _context.Campaigns.DeferredSingleOrDefault(c => c.Id == id).FromCacheAsync(options);

            }
            if (campaignToUpdate is null)
            {
                return NotFound();
            }

            campaignToUpdate.Name = campaignDto.Name;
            campaignToUpdate.Description = campaignDto.Description;
            campaignToUpdate.From = campaignDto.From;
            campaignToUpdate.To = campaignDto.To;
            campaignToUpdate.PictureUri = campaignDto.PictureUri;

            await _context.SaveChangesAsync();

            if (_settings.CachingEnabled)
            {
                var cache = _context.GetCache();
                var options = new CachingOptions();
                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.MarketingCacheExpirationTimeInMinutes));
                await Task.Run(() => cache.Insert(campaignToUpdate, out string cacheKey, options));
            }

            return CreatedAtAction(nameof(GetCampaignByIdAsync), new { id = campaignToUpdate.Id }, null);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteCampaignByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            Campaign campaignToDelete = null;

            if (!_settings.CachingEnabled)
            {
                campaignToDelete = await _context.Campaigns.FindAsync(id); 
            }
            else
            {
                var options = new CachingOptions();

                campaignToDelete = await _context.Campaigns.DeferredSingleOrDefault(c => c.Id == id).FromCacheAsync(options);
            }

            if (campaignToDelete is null)
            {
                return NotFound();
            }

            _context.Campaigns.Remove(campaignToDelete);
            await _context.SaveChangesAsync();

            if (_settings.CachingEnabled)
            {
                var cache = _context.GetCache();
                await Task.Run(() => cache.Remove(campaignToDelete));
            }

            return NoContent();
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CampaignDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CampaignDTO>>> GetCampaignsByUserIdAsync( int pageSize = 10, int pageIndex = 0)
        {
            var userId = _identityService.GetUserIdentity();

            var marketingData = await _marketingDataRepository.GetAsync(userId.ToString());

            var campaignDtoList = new List<CampaignDTO>();
            
            if (marketingData != null)
            {
                var locationIdCandidateList = marketingData.Locations.Select(x => x.LocationId);
                var userCampaignList = await _context.Rules
                    .OfType<UserLocationRule>()
                    .Include(c => c.Campaign)
                    .Where(c => c.Campaign.From <= DateTime.Now
                                && c.Campaign.To >= DateTime.Now
                                && locationIdCandidateList.Contains(c.LocationId))
                                    .Select(c => c.Campaign)
                                    .ToListAsync();

                if (userCampaignList != null && userCampaignList.Any())
                {
                    var userCampaignDtoList = MapCampaignModelListToDtoList(userCampaignList);
                    campaignDtoList.AddRange(userCampaignDtoList);
                }
            }

            var totalItems = campaignDtoList.Count();

            campaignDtoList = campaignDtoList
                .Skip(pageSize * pageIndex)
                .Take(pageSize).ToList();

            return new PaginatedItemsViewModel<CampaignDTO>(pageIndex, pageSize, totalItems, campaignDtoList);
        }

        private List<CampaignDTO> MapCampaignModelListToDtoList(List<Campaign> campaignList)
        {
            var campaignDtoList = new List<CampaignDTO>();

            campaignList.ForEach(campaign => campaignDtoList
                .Add(MapCampaignModelToDto(campaign)));

            return campaignDtoList;
        }

        private CampaignDTO MapCampaignModelToDto(Campaign campaign)
        {
            var userId = _identityService.GetUserIdentity();
            var dto = new CampaignDTO
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,
                From = campaign.From,
                To = campaign.To,
                PictureUri = GetUriPlaceholder(campaign),
            };

            return dto;
        }

        private Campaign MapCampaignDtoToModel(CampaignDTO campaignDto)
        {
            return new Campaign
            {
                Id = campaignDto.Id,
                Name = campaignDto.Name,
                Description = campaignDto.Description,
                From = campaignDto.From,
                To = campaignDto.To,
                PictureUri = campaignDto.PictureUri
            };
        }

        private string GetUriPlaceholder(Campaign campaign)
        {
            var baseUri = _settings.PicBaseUrl;

            return baseUri.Replace("[0]", campaign.Id.ToString());
        }
    }
}