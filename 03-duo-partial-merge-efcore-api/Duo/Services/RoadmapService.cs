using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.Models.Roadmap;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class RoadmapService : IRoadmapService
    {
        private IRoadmapServiceProxy serviceProxy;

        public RoadmapService(IRoadmapServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        // public async Task<List<Roadmap>> GetAllAsync()
        // {
        //        return await serviceProxy.GetAllAsync();
        // }
        public async Task<Roadmap> GetByIdAsync(int roadmapId)
        {
                return await serviceProxy.GetByIdAsync(roadmapId);
        }

        // public async Task<Roadmap> GetByNameAsync(string roadmapName)
        // {
        //        return await serviceProxy.GetByNameAsync(roadmapName);
        // }

        // public async Task<int> AddAsync(Roadmap roadmap)
        // {
        //        return await serviceProxy.AddAsync(roadmap);
        // }

        // public async Task DeleteAsync(Roadmap roadmap)
        // {
        //        await serviceProxy.DeleteAsync(roadmap);
        // }
    }
}
