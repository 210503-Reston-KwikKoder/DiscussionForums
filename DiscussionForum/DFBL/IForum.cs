using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBL
{
    class IForum
    {
        Task<List<Forum>> GetAllForums();
        /// <summary>
        /// Returns a Forum Object with the given ForumID
        /// </summary>
        /// <returns></returns>
        Task<Forum> GetForum(int forumID);
        /// <summary>
        /// Creates a Forum in the Database, based off of the Forum object passed
        /// </summary>
        /// <returns></returns>
        Task<Forum> AddForum(Forum forum);
        /// <summary>
        /// Removes a Forum from the Database for the given Post 
        /// </summary>
        /// <returns></returns>
        Task<Forum> RemoveForum(Forum forum);
        /// <summary>
        /// Updates the given Forum Object in the Database
        /// </summary>
        /// <returns></returns>
        Task<Forum> UpdateForum(Forum forum);
        /// <summary>
        /// Returns all Likes in Database
        /// </summary>
        /// <returns></returns>
    }
}
