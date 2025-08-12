using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Roadmap;

namespace DuoClassLibrary.Services
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