using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoxOwlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BoxOwlWeb.API {

    [Route("api/couriers")]
    [ApiController]
    public class CourierController : ControllerBase {

        private readonly BoxOwlDbContext _context;

        public CourierController(BoxOwlDbContext context) {
            _context = context;
        }

        // GET: api/couriers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Courier>>> GetCourier() {
            return await _context.Courier.ToListAsync();
        }

        // GET: api/couriers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Courier>> GetCourier(int id) {
            var user = await _context.Courier.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/couriers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Courier>> PutCourier(int id, Courier courier) {
            if (id != courier.CourierId) {
                return BadRequest();
            }
            _context.Entry(courier).State = EntityState.Modified;
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        private bool UserExists(int id) {
            return _context.Courier.Any(e => e.CourierId == id);
        }

    }
}
