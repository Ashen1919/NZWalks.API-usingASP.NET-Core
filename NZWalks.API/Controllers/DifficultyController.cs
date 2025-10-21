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
    public class DifficultyController : ControllerBase
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        private readonly IDifficultyRepository difficultyRepository;
        private readonly IMapper mapper;

        public DifficultyController(NZWalksDBContext nZWalksDBContext, IDifficultyRepository difficultyRepository, IMapper mapper)
        {
            this.nZWalksDBContext = nZWalksDBContext;
            this.difficultyRepository = difficultyRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDifficulty([FromBody] AddDifficultyRequestDto addDifficultyRequestDto)
        {
            var difficultyDomainModel = mapper.Map<Difficulty>(addDifficultyRequestDto);

            difficultyDomainModel = await difficultyRepository.CreateDifficultyAsync(difficultyDomainModel);

            var difficultyDto = mapper.Map<DifficultyDto>(difficultyDomainModel);

            return Ok(difficultyDto);
        }
    }
}
