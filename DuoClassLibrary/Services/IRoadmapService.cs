using DuoClassLibrary.Models.Roadmap;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DuoClassLibrary.Services
{
    public interface IRoadmapService
    {
        // Task<int> AddAsync(Roadmap roadmap);
        // Task DeleteAsync(Roadmap roadmap);
        // Task<List<Roadmap>> GetAllAsync();
        // Task<Roadmap> GetByNameAsync(string roadmapName);
        Task<Roadmaps> GetByIdAsync(int roadmapId);
    }
}