using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Roadmaps;
using Duo.Api.Persistence;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class RoadmapsController(IRepository repository) : BaseController(repository)
    {
        private readonly DataContext context;

        // GET: api/Roadmaps
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Roadmap>>> GetRoadmap()
        //{
        //    return await repository.GetRoadmapsFromDbAsync();
        //}

        // GET: api/Roadmaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Roadmap>> GetRoadmap(int id)
        {
            var roadmap = await repository.GetRoadmapByIdAsync(id);

            if (roadmap == null)
            {
                return NotFound();
            }

            return roadmap;
        }

        // GET: api/Roadmaps?name=example
        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<Roadmap>>> GetRoadmapsByName(string name)
        //{
        //    var roadmaps = await repository.GetRoadmapsByNameAsync(name);
        //    if (roadmaps == null || !roadmaps.Any())
        //    {
        //        return NotFound();
        //    }

        //    return roadmaps;
        //}

        // PUT: api/Roadmaps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRoadmap(int id, Roadmap roadmap)
        //{
        //    if (id != roadmap.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(roadmap).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RoadmapExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Roadmaps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Roadmap>> PostRoadmap(Roadmap roadmap)
        //{
        //    context.Roadmaps.Add(roadmap);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction("GetRoadmap", new { id = roadmap.Id }, roadmap);
        //}

        // DELETE: api/Roadmaps/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRoadmap(int id)
        //{
        //    var roadmap = await context.Roadmaps.FindAsync(id);
        //    if (roadmap == null)
        //    {
        //        return NotFound();
        //    }

        //    context.Roadmaps.Remove(roadmap);
        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool RoadmapExists(int id)
        //{
        //    return context.Roadmaps.Any(e => e.Id == id);
        //}
    }
}
