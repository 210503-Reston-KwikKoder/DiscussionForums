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
    public class PostController : ControllerBase
    {
        private readonly IBussiness _BL;

        public PostController (IBussiness BL)
        {
            _BL = BL;
        }
        // GET: api/<DogController>
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                return Ok(await _BL.GetAllPosts());
            } catch (Exception e)
            {
                Log.Error("Failed to Get all posts in PostController", e.Message);
                return NotFound();
            }
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                return Ok(await _BL.GetPostForForumWithID(id));
            }
            catch (Exception e)
            {
                Log.Error("Failed to Get post with ID: " + id + " in PostController", e.Message);
                return NotFound();
            }
        }

        // PUT api/<DogController>
        [HttpPost]
        public async Task<IActionResult> AddPost(Posts post)
        {
            try
            {
                return Created( "api/Post",await _BL.AddPost(post));
            }
            catch (Exception e)
            {
                Log.Error("Failed to add post with ID: " + post.PostID + " in PostController", e.Message);
                return BadRequest();
            }
        }

        // POST api/<DogController>
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] Posts post)
        {
            try
            {
                await _BL.UpdatePost(post);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to update post with ID: " + post.PostID + " in PostController", e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Posts post)
        {
            try
            {
                await _BL.RemovePost(post);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Delete post with ID: " + post.PostID + " in PostController", e.Message);
                return BadRequest();
            }
        }
    }
}
