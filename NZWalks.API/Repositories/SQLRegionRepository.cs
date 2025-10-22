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

        public async Task<List<Region>> GetRegionsAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAcsending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //Filtering 
            var regions = dBContext.Regions.AsQueryable();

            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAcsending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAcsending ? regions.OrderBy(x => x.Code) : regions.OrderByDescending(x => x.Code);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await regions.Skip(skipResults).Take(pageSize).ToListAsync();
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
