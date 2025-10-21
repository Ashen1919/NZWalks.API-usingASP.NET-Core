using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IDifficultyRepository
    {
        Task<Difficulty> CreateDifficultyAsync(Difficulty difficulty);
    }
}
