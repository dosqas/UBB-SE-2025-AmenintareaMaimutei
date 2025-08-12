using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Models.Roadmap;

namespace Duo.Services.Interfaces
{
    public interface IRoadmapServiceProxy
    {
        // Task<int> AddAsync(Roadmap roadmap);
        // Task DeleteAsync(Roadmap roadmap);
        // Task<List<Roadmap>> GetAllAsync();
        Task<Roadmap> GetByIdAsync(int roadmapId);
        // Task<Roadmap> GetByNameAsync(string roadmapName);
    }
}