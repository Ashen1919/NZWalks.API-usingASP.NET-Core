using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext dBContext;

        public SQLRegionRepository(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await dBContext.Regions.AddAsync(region);
            await dBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            var existingRegion = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null) 
            {
                return null;
            }

            dBContext.Regions.Remove(existingRegion);
            await dBContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> GetByIdAsync(Guid Id)
        {
            return await dBContext.Regions.FirstOrDefaultAsync(x =>  x.Id == Id);
        }

        public async Task<List<Region>> GetRegionsAsync()
        {
            return await dBContext.Regions.ToListAsync();
        }

        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
        {
            var ExistingRegion = await dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (ExistingRegion == null) {
                return null;
            }

            ExistingRegion.Code = region.Code;
            ExistingRegion.Name = region.Name;
            ExistingRegion.RegionImageUrl = region.RegionImageUrl;

            await dBContext.SaveChangesAsync();
            return ExistingRegion;
        }
    }
}
