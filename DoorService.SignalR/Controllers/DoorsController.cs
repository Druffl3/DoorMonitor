using DoorService.SignalR.Contexts;
using DoorService.SignalR.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoorService.SignalR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoorsController : ControllerBase
    {
        /// <summary>
        /// The Door database context
        /// </summary>
        private readonly DoorDbContext _context;

        public DoorsController(DoorDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/doors/getAllDoors
        /// </summary>
        /// <returns>The list of all the doors in the database</returns>
        [HttpGet("getAllDoors")]
        public async Task<ActionResult<IEnumerable<Door>>> GetAllDoors()
        {
            try
            {
                return await _context.DoorTable.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = $"{ex.Message}"
                });
            }
        }
    }
}
