using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetRegionsAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAcsending = true, int pageNumber = 1, int pageSize = 1000); 
        Task<Region?> GetByIdAsync(Guid Id);
        Task<Region> CreateRegionAsync(Region region);
        Task<Region?> UpdateRegionAsync(Guid id, Region region);
        Task<Region?> DeleteRegionAsync(Guid id);
    }
}
