using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;

namespace DFDL
{
    public interface IRepo
    {
        //Forums
        Task<Forum> AddForumAsync(Forum forum);
        Task<int> DeleteForumAsync(int forumID);
        Task<Forum> UpdateForumAsync(Forum forum);
        Task<Forum> GetForumByIdAsync(int id);
        Task<List<Forum>> GetAllForumsAsync();
        Task<Forum> GetForumAsync(Forum forum);
        //Posts
        Task<Posts> AddPostsAsync(Posts posts);
        Task<int> DeletePostsAsync(Posts post);
        Task<Posts> UpdatePostsAsync(Posts posts);
        Task<Posts> GetPostsByIdAsync(int id);
        Task<List<Posts>> GetPostForForumWithID(int id);
        Task<Posts> GetPostByPostID(int id);
        Task<List<Posts>> GetAllPostsAsync();
        Task<Posts> GetPostsAsync(Posts posts);
        //Comments
        Task<Comments> AddCommentsAsync(Comments comments);
        Task<int> DeleteCommentsAsync(Comments comment);
        Task<Comments> UpdateCommentsAsync(Comments comments);
        Task<List<Comments>> GetCommentsByIdAsync(int Postid);
        Task<Comments> GetCommentByCommentID(int CommentID);
        Task<List<Comments>> GetAllCommentsAsync();
        Task<Comments> GetCommentsAsync(Comments comments);
    }
}
