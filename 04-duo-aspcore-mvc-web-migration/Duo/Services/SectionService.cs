using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Sections;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    /// <summary>
    /// Provides methods to interact with sections, delegating to the SectionServiceProxy.
    /// </summary>
    public class SectionService : ISectionService
    {
        private readonly ISectionServiceProxy sectionServiceProxy;

        /// <summary>
        /// Creates a new instance of SectionService using the given proxy.
        /// </summary>
        public SectionService(ISectionServiceProxy sectionServiceProxy)
        {
            this.sectionServiceProxy = sectionServiceProxy ?? throw new ArgumentNullException(nameof(sectionServiceProxy));
        }

        /// <summary>
        /// Optional overload to allow passing the concrete proxy directly.
        /// </summary>
        public SectionService(SectionServiceProxy concreteProxy)
            : this((ISectionServiceProxy)concreteProxy)
        {
        }

        public async Task<int> AddSection(Section section)
        {
            // ValidationHelper.ValidateSection(section);
            var allSections = await GetAllSections();
            section.OrderNumber = allSections.Count + 1;
            return await sectionServiceProxy.AddSection(section);
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            return await sectionServiceProxy.CountSectionsFromRoadmap(roadmapId);
        }

        public async Task DeleteSection(int sectionId)
        {
            await sectionServiceProxy.DeleteSection(sectionId);
        }

        public async Task<List<Section>> GetAllSections()
        {
            return await sectionServiceProxy.GetAllSections();
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            return await sectionServiceProxy.GetByRoadmapId(roadmapId);
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            return await sectionServiceProxy.GetSectionById(sectionId);
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            return await sectionServiceProxy.LastOrderNumberFromRoadmap(roadmapId);
        }

        public async Task UpdateSection(Section section)
        {
            ValidationHelper.ValidateSection(section);
            await sectionServiceProxy.UpdateSection(section);
        }

        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            return await sectionServiceProxy.TrackCompletion(sectionId, isCompleted);
        }

        public async Task<bool> ValidateDependencies(int sectionId)
        {
            var dependencies = await sectionServiceProxy.GetSectionDependencies(sectionId);
            foreach (var dependency in dependencies)
            {
                if (!dependency.IsCompleted)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
