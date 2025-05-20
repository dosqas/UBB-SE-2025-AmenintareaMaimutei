using DuoClassLibrary.Models.Roadmap;


namespace DuoClassLibrary.Services.Interfaces
{
    public interface IRoadmapServiceProxy
    {
        // Task<int> AddAsync(Roadmap roadmap);
        // Task DeleteAsync(Roadmap roadmap);
        // Task<List<Roadmap>> GetAllAsync();
        Task<Roadmaps> GetByIdAsync(int roadmapId);
        // Task<Roadmap> GetByNameAsync(string roadmapName);
    }
}