using Microsoft.EntityFrameworkCore;
using System.Threading;
using TMG_Site_API.Models;

namespace TMG_Site_API.Data
{
    interface ITmgPoolDataService
    {

        public  Task<List<TmgPrice>> GetAllPrices();

    }

    public class TmgPoolDataService(IDbContextFactory<TmgPoolApiContext> contextFactory) : ITmgPoolDataService
    {


            private readonly IDbContextFactory<TmgPoolApiContext> _contextFactory = contextFactory;

        public async Task<List<TmgPrice>> GetAllPrices()

        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.TmgPrices.OrderByDescending(o => o.Epoch).ToListAsync<TmgPrice>();

            }

        }


    }
}
