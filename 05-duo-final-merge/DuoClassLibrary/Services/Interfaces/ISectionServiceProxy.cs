
using DuoClassLibrary.Models.Sections;

namespace DuoClassLibrary.Services.Interfaces
{
    public interface ISectionServiceProxy
    {
        Task<int> AddSection(Section section);
        Task<int> CountSectionsFromRoadmap(int roadmapId);
        Task DeleteSection(int sectionId);
        Task<List<Section>> GetAllSections();
        Task<List<Section>> GetByRoadmapId(int roadmapId);
        Task<Section> GetSectionById(int sectionId);
        Task<int> LastOrderNumberFromRoadmap(int roadmapId);
        Task UpdateSection(Section section);
        Task<bool> IsSectionCompleted(int userId, int sectionId);
        Task CompleteSection(int userId, int sectionId);
    }
}
