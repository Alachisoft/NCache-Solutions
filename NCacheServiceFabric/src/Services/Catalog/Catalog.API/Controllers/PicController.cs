using Alachisoft.NCache.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Catalog.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers
{
    [ApiController]
    public class PicController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        private readonly CatalogSettings _settings;
        private readonly CatalogContext _catalogContext;

        public PicController(IHostingEnvironment env,
            IOptionsSnapshot<CatalogSettings> settings,
            CatalogContext catalogContext)
        {
            _env = env;
            _catalogContext = catalogContext;
            _settings = settings.Value;
        }

        [HttpGet]
        [Route("api/v1/catalog/items/{catalogItemId:int}/pic")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // GET: /<controller>/
        public async Task<ActionResult> GetImageAsync(int catalogItemId)
        {
            if (catalogItemId <= 0)
            {
                return BadRequest();
            }

            CatalogItem item = null;
            if (!_settings.EFCoreCachingEnabled)
            {
                item = await _catalogContext.CatalogItems
                        .SingleOrDefaultAsync(ci => ci.Id == catalogItemId); 
            }
            else
            {
                var options = new CachingOptions
                {
                //    QueryIdentifier = $"ItemByIdAsync-{catalogItemId}"
                };

                options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(_settings.NCacheAbsoluteExpirationTime));

                item = await _catalogContext.CatalogItems.DeferredSingleOrDefault(ci => ci.Id == catalogItemId).FromCacheAsync(options);
            }


            if (item != null)
            {
                var webRoot = _env.WebRootPath;
                var path = Path.Combine(webRoot, item.PictureFileName);

                string imageFileExtension = Path.GetExtension(item.PictureFileName);
                string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

                var buffer = System.IO.File.ReadAllBytes(path);

                return File(buffer, mimetype);
            }

            return NotFound();
        }

        private string GetImageMimeTypeFromImageFileExtension(string extension)
        {
            string mimetype;

            switch (extension)
            {
                case ".png":
                    mimetype = "image/png";
                    break;
                case ".gif":
                    mimetype = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimetype = "image/jpeg";
                    break;
                case ".bmp":
                    mimetype = "image/bmp";
                    break;
                case ".tiff":
                    mimetype = "image/tiff";
                    break;
                case ".wmf":
                    mimetype = "image/wmf";
                    break;
                case ".jp2":
                    mimetype = "image/jp2";
                    break;
                case ".svg":
                    mimetype = "image/svg+xml";
                    break;
                default:
                    mimetype = "application/octet-stream";
                    break;
            }

            return mimetype;
        }
    }
}
