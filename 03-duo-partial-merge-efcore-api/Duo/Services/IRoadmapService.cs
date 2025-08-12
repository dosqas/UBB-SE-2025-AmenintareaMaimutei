using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services
{
    public interface IRoadmapService
    {
        // Task<int> AddAsync(Roadmap roadmap);
        // Task DeleteAsync(Roadmap roadmap);
        // Task<List<Roadmap>> GetAllAsync();
        // Task<Roadmap> GetByNameAsync(string roadmapName);
        Task<Roadmap> GetByIdAsync(int roadmapId);
    }
}