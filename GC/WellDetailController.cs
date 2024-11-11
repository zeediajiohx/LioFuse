using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pseven.Models.well;
//sing pseven.Models;

namespace pseven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WELLController : ControllerBase
    {
        private readonly WellContext _context;

        public WELLController(WellContext context)
        {
            _context = context;
        }

        // GET: api/WELL_lite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Well>>> GetWellDetail()
        {
            return await _context.Wellitems.ToListAsync();
        }

        // GET: api/WELL_lite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Well>> GetWELLDetail(string id)
        {
            var wELLitems = await _context.Wellitems.FindAsync(id);

            if (wELLitems == null)
            {
                return NotFound();
            }

            return wELLitems;
        }

        // PUT: api/WELL_lite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWELLDetail(string id, Well wELL_lite)
        {
            if (id != wELL_lite.Id)
            {
                return BadRequest();
            }

            _context.Entry(wELL_lite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WELL_liteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WELL_lite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Well>> PostWELL_lite(Well wELL_lite)
        {
            _context.Wellitems.Add(wELL_lite);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WELL_liteExists(wELL_lite.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWELL_lite", new { id = wELL_lite.Id }, wELL_lite);
        }

        // DELETE: api/WELL_lite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWELL_lite(string id)
        {
            var wELL_lite = await _context.Wellitems.FindAsync(id);
            if (wELL_lite == null)
            {
                return NotFound();
            }

            _context.Wellitems.Remove(wELL_lite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WELL_liteExists(string id)
        {
            return _context.Wellitems.Any(e => e.Id == id);
        }
    }
}
