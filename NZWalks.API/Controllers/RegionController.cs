using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(NZWalksDBContext dBContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //Get all regions
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetRegions([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //get data from database - Domain models
            var regionsDomain = await regionRepository.GetRegionsAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Map domain models to DTOs
            var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //Return DTOs
            return Ok(regionDto);
        }

        //Get single region by ID
        [HttpGet]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get Region Domain Model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            //Map Region Domain model to DTO And Return it
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //Create region
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
                //Map or convert DTO to domain model
                var RegionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //use domain model to create region
                RegionDomainModel = await regionRepository.CreateRegionAsync(RegionDomainModel);

                //Map Domain model back to DTO
                var regionDto = mapper.Map<RegionDto>(RegionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = RegionDomainModel.Id }, RegionDomainModel);
        }

        //Update region
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
                //Map DTO to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                //checkif region exist
                regionDomainModel = await regionRepository.UpdateRegionAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Convert Domain Model to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteRegionAsync(id);

            if( regionDomainModel == null)
            {
                return NotFound();
            }
            //Map domain model to DTO
            var regionDto = mapper.Map<Region>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
