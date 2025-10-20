using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;

        public RegionController(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        //Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegion()
        {
            //get data from database - Domain models
            var regionsDomain = await dBContext.Regions.ToListAsync();

            //Map domain models to DTOs
            var regionDto = new List<RegionDto>();
            foreach (var region in regionsDomain) 
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

            //Return DTOs
            return Ok(regionDto);
        }

        //Get single region by ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get Region Domain Model from database
            var regionDomain = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain model to DTO
            var regionsDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionsDto);
        }

        //Create region
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to domain model
            var RegionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };

            //use domain model to create region
            await dBContext.Regions.AddAsync(RegionDomainModel);
            await dBContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = RegionDomainModel.Id,
                Code = RegionDomainModel.Code,
                Name = RegionDomainModel.Name,
                RegionImageUrl = RegionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof (GetById), new { id =  RegionDomainModel.Id }, RegionDomainModel);
        }

        //Update region
        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //checkif region exist
            var regionDomainModel = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

             await dBContext.SaveChangesAsync();

            //Convert Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id) 
        {
            var regionDomainModel = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if( regionDomainModel == null)
            {
                return NotFound();
            }

            dBContext.Regions.Remove(regionDomainModel);
            await dBContext.SaveChangesAsync();

            return Ok();
        }

    }
}
