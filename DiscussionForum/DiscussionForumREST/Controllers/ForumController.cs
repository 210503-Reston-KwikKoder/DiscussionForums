using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFBL;
using DFDL;
using DFModels;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscussionForumREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForum _BL;

        public ForumController(IForum BL)
        {
            _BL = BL;
        }
        // GET: api/<DogController>
        [HttpGet]
        public async Task<IActionResult> GetForums()
        {
            try
            {
                return Ok(await _BL.GetAllForums());
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve all Forums in ForumController", e.Message);
                return NotFound();
            }
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForum(int id)
        {
            try
            {
                return Ok(await _BL.GetForum(id));
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve all Forums with ID: " + id + " in ForumController", e.Message);
                return NotFound();
            }
        }

        // PUT api/<DogController>
        [HttpPost]
        public async Task<IActionResult> AddForum(Forum forum)
        {
            try
            {
                await _BL.AddForum(forum);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to add Forums with ID: " + forum.ForumID + " in ForumController", e.Message);
                return BadRequest();
            }
        }

        // POST api/<DogController>
        [HttpPut]
        public async Task<IActionResult> UpdateForum([FromBody] Forum forum)
        {
            try
            {
                await _BL.UpdateForum(forum);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to update Forums with ID: " + forum.ForumID + " in ForumController", e.Message);
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            try
            {
                await _BL.RemoveForum(id);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to remove Forums with ID: " + id + " in ForumController", e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
