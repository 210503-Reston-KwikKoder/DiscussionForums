using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFBL;
using DFModels;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using DTO = DiscussionForumREST.DTO;
using System.Security.Claims;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Auth0.ManagementApi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscussionForumREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumPostController : ControllerBase
    {
        private readonly IForumPost _BL;
        private readonly ApiSettings _ApiSettings;

        public ForumPostController(IForumPost BL, IOptions<ApiSettings> settings)
        {
            _BL = BL;
            _ApiSettings = settings.Value;
        }
        // GET: api/<DogController>
        [HttpGet]
        [Authorize("read:messages")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                return Ok(await _BL.GetAllPosts());
            }
            catch (Exception e)
            {
                Log.Error("Failed to Get all posts in PostController", e.Message);
                return NotFound();
            }
        }

        // GET api/<DogController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                // Gathers the Token from request header bearer and calls Auth0 API to gather info
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken;
                IRestResponse restResponse;
                dynamic deResponse;

                // Get results from backend
                List<Posts> found = await _BL.GetPostForForumWithID(id);

                // Translate Posts into expected output with isUser representing if the Post belongs to the user
                List<DTO.ForumPostOutput> translated = new List<DTO.ForumPostOutput>();
                foreach(Posts post in found)
                {
                    string PostUserID = post.AuthID;
                    AppBearerToken = GetApplicationToken();
                    /*ManagementApiClient Authclient = new ManagementApiClient(AppBearerToken.access_token, new Uri("https://kwikkoder.us.auth0.com/api/v2/"));*/
                    var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{PostUserID}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                    restResponse = await client.ExecuteAsync(request);
                    /*var Response = Authclient.Users.GetAsync(PostUserID).Result;*/
                    deResponse = JsonConvert.DeserializeObject(restResponse.Content);

                    DTO.ForumPostOutput temp = new DTO.ForumPostOutput()
                    {
                        DateCreated = post.DateCreated,
                        Description = post.Description,
                        ForumID = id,
                        PostID = post.PostID,
                        UserName = deResponse.name,
                        ImgURL = deResponse.picture,
                        Topic = post.Topic,
                        isUser = (UserID == post.AuthID)
                    };
/*                    if (deResponse.username == null)
                        temp.UserName = this.deResponse.name;
                    else
                        temp.UserName = this.deResponse.username;*/
                    translated.Add(temp);
                }
                // Return the translation
                return Ok(translated);
            }
            catch (Exception e)
            {
                Log.Error("Failed to Get post with ID: " + id + " in PostController", e.Message);
                return BadRequest(e.Message);
            }
        }

        // PUT api/<DogController>
        [HttpPost]
         [Authorize]
        public async Task<IActionResult> AddPost(DTO.AddForumPostInput input)
        {
            try
            {
                // Gathers the Token from request header bearer and calls Auth0 API to gather info
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                dynamic AppBearerToken = GetApplicationToken();
                var client = new RestClient($"https://kwikkoder.us.auth0.com/api/v2/users/{UserID}");
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + AppBearerToken.access_token);
                IRestResponse restResponse = await client.ExecuteAsync(request);
                dynamic deResponse = JsonConvert.DeserializeObject(restResponse.Content);

                // Creates a post object with given input
                Posts post = new Posts()
                {
                    AuthID = UserID,
                    Description = input.Description,
                    ForumID = input.ForumID,
                    Topic = input.Topic,
                    PostID = 0,
                    DateCreated = DateTime.Now
                };
                // Sets to UserName if it exists
                if (deResponse.UserName == null)
                    post.UserName = deResponse.name;
                else
                    post.UserName = deResponse.username;

                return Created("api/Post", await _BL.AddPost(post));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Error("Failed to add post with ForumID: " + input.ForumID + " in PostController", e.Message);
                return BadRequest();
            }
        }

        // POST api/<DogController>
        [HttpPut]
         [Authorize]
        public async Task<IActionResult> UpdatePost(DTO.ForumPostInput input)
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
                dynamic deResponse  = JsonConvert.DeserializeObject(restResponse.Content);

                // Tracks currently stored value and updates their values
                Posts relatedPost = await _BL.GetPostByPostID(input.PostID);
                relatedPost.UserName = deResponse.UserName;
                relatedPost.Description = input.Description;
                relatedPost.Topic = input.Topic;

                // Returns to be updated
                await _BL.UpdatePost(relatedPost);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to update post with ID: " + input.PostID + " in PostController", e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
         [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                string UserID = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Posts toDelete = await _BL.GetPostByPostID(id);
                if (toDelete.AuthID == UserID)
                    await _BL.RemovePost(toDelete);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error("Failed to Delete post with ID: " + id + " in PostController", e.Message);
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
