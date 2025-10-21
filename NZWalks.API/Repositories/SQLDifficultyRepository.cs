using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLDifficultyRepository : IDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public SQLDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Difficulty> CreateDifficultyAsync(Difficulty difficulty)
        {
            await nZWalksDBContext.Difficulties.AddAsync(difficulty);
            await nZWalksDBContext.SaveChangesAsync();
            return difficulty;
        }
    }
}
