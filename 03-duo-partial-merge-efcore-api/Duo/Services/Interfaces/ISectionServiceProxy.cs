using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Sections;

namespace Duo.Services.Interfaces
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
        Task<bool> TrackCompletion(int sectionId, bool isCompleted);
        Task<List<SectionDependency>> GetSectionDependencies(int sectionId);
    }
}
