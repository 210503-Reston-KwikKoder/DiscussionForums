using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;

namespace DFBL
{
    public interface IForumPost
    {
        /// <summary>
        /// Returns all Posts in Database
        /// </summary>
        /// <returns></returns>
        Task<List<Posts>> GetAllPosts();
        /// <summary>
        /// Creates a Post in the Database, based off of the Post object passed
        /// </summary>
        /// <returns></returns>
        Task<Posts> AddPost(Posts post);
        /// <summary>
        /// Returns a Post Object with the given forumID
        /// </summary>
        /// <param name="id">forumID</param>
        /// <returns></returns>
        Task<List<Posts>> GetPostForForumWithID(int id);
        /// <summary>
        /// Removes a Post from the Database for the given Post 
        /// </summary>
        /// <returns></returns>
        Task<Posts> RemovePost(Posts post);
        /// <summary>
        /// Updates the given Post Object in the Database
        /// </summary>
        /// <returns></returns>
        Task<Posts> UpdatePost(Posts post);
        /// <summary>
        /// Returns all Forums in Database
        /// </summary>
        /// <returns></returns>
    }
}
