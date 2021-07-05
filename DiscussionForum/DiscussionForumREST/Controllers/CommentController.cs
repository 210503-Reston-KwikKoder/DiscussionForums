using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFDL;
using DFBL;
using DFModels;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscussionForumREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IComment _BL;
        private readonly ApiSettings _ApiSettings;

        public CommentController(IComment BL, IOptions<ApiSettings> settings)
        {
            _BL = BL;
            _ApiSettings = settings.Value;
        }
        // GET: api/<CommentController>
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {           
            try
            {
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                List<Comments> DBCommnets = await _BL.GetAllComments();
                List<DTO.CommentOutput> OutputComments = new List<DTO.CommentOutput>();
                foreach(Comments c in DBCommnets)
                {
                    DTO.CommentOutput co = new DTO.CommentOutput();
                    co.AuthID = c.AuthID;
                    co.CommentID = c.CommentID;
                    co.Created = c.Created;
                    co.isLoggedUser = (c.AuthID == UserID);
                    co.PostID = c.PostID;
                    co.Message = c.Message;
                    co.UserName = c.UserName;
                    OutputComments.Add(co);
                }
                return Ok(OutputComments);
            }
            catch (Exception e)
            {
                Log.Error("Failed to gather all comments In CommentController", e.Message);
                return NotFound();
            }
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetComment(int id)
        {
            try
            {
                // Gathers the Token from request header bearer and calls Auth0 API to gather info
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);

                // Get results from backend
                List<Comments> found = await _BL.GetCommentsByPostID(id);

                // Translate Posts into expected output with isUser representing if the Post belongs to the user
                List<DTO.CommentOutput> translated = new List<DTO.CommentOutput>();
                foreach (Comments c in found)
                {
                    DTO.CommentOutput temp = new DTO.CommentOutput()
                    {
                        Created = c.Created,
                        CommentID = c.CommentID,
                        isLoggedUser = (c.AuthID == UserID),
                        PostID = c.PostID,
                        AuthID = c.AuthID,
                        Message = c.Message
                    };
                    
                    if (deResponse.username == null)
                        temp.UserName = deResponse.name;
                    else
                        temp.UserName = deResponse.username;
                    translated.Add(temp);

                }
                // Return the translation
                return Ok(translated);
            }
            catch (Exception e)
            {
                Log.Error("Failed to gather comments for ID: " + id + " In CommentController", e.Message);
                return NotFound();
            }

        }

        // PUT api/<CommentController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(DTO.AddCommnetInput comm)
        {
            Comments _comm = new Comments();
            try
            {
                // Gathers the Token from request header bearer and calls Auth0 API to gather info
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);
                _comm.PostID = comm.PostID;
                _comm.AuthID = UserID;
                _comm.Created = DateTime.Now;
                _comm.Message = comm.Message;
                if(deResponse.UserName == null)
                {
                    _comm.UserName = deResponse.name;
                }
                else
                {
                    _comm.UserName = deResponse.username;
                }
                return Created("api/Comment", await _BL.AddComment(_comm));
            }
            catch (Exception e)
            {
                Log.Error("Failed to add comments for ID: " + _comm.CommentID + " In CommentController", e.Message);
                return BadRequest();
            }
        }

        // POST api/<CommentController>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromBody] DTO.UpdateCommentInput comm)
        {
            
            try
            {
                //Gathers the Token from request header bearer and calls Auth0 API to gather info
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
                var request = new RestRequest(Method.PUT);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);
                Comments _comm = new Comments();
                _comm.CommentID = comm.CommentID;
                _comm.PostID = comm.PostID;
                _comm.AuthID = UserID;
                _comm.Created = DateTime.Now;
                _comm.Message = comm.Message;
                await _BL.UpdateComment(_comm);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Update comments for ID: " + comm.CommentID + " In CommentController", e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)

        {
            try
            {
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Comments toDelete = await _BL.GetCommentByID(id);                
                    if (toDelete.AuthID == UserID)
                        await _BL.RemoveComments(toDelete);        
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Delete comments for ID: " + id + " In CommentController", e.Message);
                return BadRequest();
            }
        }
        private dynamic GetApplicationToken()
        {
            var client = new RestClient("https://kwikkoder.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", _ApiSettings.authString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Log.Information("Response: {0}", response.Content);
            return JsonConvert.DeserializeObject(response.Content);
        }
    }
}
