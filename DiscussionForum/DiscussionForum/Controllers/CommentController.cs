using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNHBL;
using DNHModels;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNHREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IBussiness _BL;

        public CommentController (IBussiness BL)
        {
            _BL = BL;
        }
        // GET: api/<DogController>
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                return Ok(await _BL.GetAllComments());
            } catch (Exception e)
            {
                Log.Error("Failed to gather all comments In CommentController", e.Message);
                return NotFound();
            }
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            try
            {
                return Ok(await _BL.GetComment(id));
            }
            catch (Exception e)
            {
                Log.Error("Failed to gather comments for ID: " + id + " In CommentController", e.Message);
                return NotFound();
            }

        }

        // PUT api/<DogController>
        [HttpPost]
        public async Task<IActionResult> AddComment(Comments comm)
        {
            try
            {
                
                return Created("api/Comment", await _BL.AddComment(comm));
            }
            catch (Exception e)
            {
                Log.Error("Failed to add comments for ID: " + comm.CommentID + " In CommentController", e.Message);
                return BadRequest();
            }
        }

        // POST api/<DogController>
        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] Comments comm)
        {
            try
            {
                await _BL.UpdateComment(comm);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Update comments for ID: " + comm.CommentID + " In CommentController", e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Comments comm)
        {
            try
            {
                await _BL.RemoveComments(comm);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Delete comments for ID: " + comm.CommentID + " In CommentController", e.Message);
                return BadRequest();
            }
        }
    }
}
