using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;

namespace DFBL
{
    public interface IComment
    {
        Task<List<Comments>> GetAllComments();
        /// <summary>
        /// Returns a Comment Object with the given commentID
        /// </summary>
        /// <returns></returns>
        Task<List<Comments>> GetCommentsByPostID(int PostID);
        Task<Comments> GetCommentByID(int CommentID);
        /// <summary>
        /// Creates a Comment in the Database, based off of the Comment object passed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Comments> AddComment(Comments comment);
        /// <summary>
        /// Removes a Comment from the Database for the given Comment 
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        Task<int> RemoveComments(Comments comment);
        /// <summary>
        /// Updates the given Comment Object in the Database
        /// </summary>
        /// <param name="user">Comment Object to be updated</param>
        /// <returns></returns>
        Task<Comments> UpdateComment(Comments comments);
    }
}
