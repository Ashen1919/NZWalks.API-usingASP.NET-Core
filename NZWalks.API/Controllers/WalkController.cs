using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController(NZWalksDBContext dBContext, IMapper mapper, IWalkRepository walkRepository)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create a walk
        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map or convert DTO to Domain Model
            var WalkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            //use domain model to create Walk
            WalkDomainModel = await walkRepository.CreateWalkAsync(WalkDomainModel);

            //Map domain model back to DTO
            var WalkDto = mapper.Map<WalkDto>(WalkDomainModel);

            return Ok(WalkDto);
        }
    }
}
