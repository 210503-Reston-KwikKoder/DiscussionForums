using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DFDL
{
    public class Repo : IRepo
    {
        //Done with Tags & starting with Forums
        private readonly DFDBContext _context;
        public Repo(DFDBContext context)
        {
            _context = context;
            Log.Debug("Repo instantiated");
        }

        public async Task<Forum> AddForumAsync(Forum forum)
        {
            await _context.Forums.AddAsync(
                forum
                );
            await _context.SaveChangesAsync();
            Log.Debug("Adding Forums into the database: {0}", forum.ForumID);
            return forum;
        }
        public async Task<int> DeleteForumAsync(int forumID)
        {
            Forum toBeDeleted = _context.Forums.AsNoTracking().First(foru => foru.ForumID == forumID);
            _context.Forums.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            Log.Debug("Removing Forums from the database: {0}", forumID);
            return forumID;
        }
        public async Task<Forum> UpdateForumAsync(Forum forum)
        {
            _context.Forums.Update(forum);
            await _context.SaveChangesAsync();
            Log.Debug("Updating Forums from the database: {0}", forum.ForumID);
            return forum;
        }
        public async Task<Forum> GetForumByIdAsync(int id)
        {
            Log.Debug("Getting Forums from the database by ID: {0}", id);
            return await _context.Forums.FindAsync(id);
        }
        public async Task<List<Forum>> GetAllForumsAsync()
        {
            Log.Debug("Getting all Forums from the database");
            return await _context.Forums.AsNoTracking()
            .Select(
                forum => forum
            ).ToListAsync();
        }

        public async Task<Forum> GetForumAsync(Forum forum)
        {
            Forum found = await _context.Forums.AsNoTracking().FirstOrDefaultAsync(foru => foru.Topic == forum.Topic);
            if (found == null)
            {
                Log.Error("Forum does not exist!");
                return null;
            }
            Log.Debug("Getting Forums from the database: {0}", forum.ForumID);
            return new Forum(found.ForumID, found.Topic, found.Description);
        }

        //Done with Forums & starting with Posts
        public async Task<Posts> AddPostsAsync(Posts posts)
        {
            try
            {
                await _context.Posts.AddAsync(
                    posts
                    );
                await _context.SaveChangesAsync();
                Log.Debug("Adding Posts into the database: {0}", posts.PostID);
                return posts;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Error("Failed to Add Post: " + posts + "\n" + e.Message);
                return null;
            }
        }
        public async Task<int> DeletePostsAsync(Posts post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            Log.Debug("Removing Posts from the database: {0}", post.PostID);
            return post.PostID;
        }
        public async Task<Posts> UpdatePostsAsync(Posts posts)
        {
            await _context.SaveChangesAsync();
            Log.Debug("Updating Posts from the database: {0}", posts.PostID);
            return posts;
        }
        public async Task<Posts> GetPostsByIdAsync(int id)
        {
            Log.Debug("Getting Posts from the database by ID: {0}", id);
            return await _context.Posts.FindAsync(id);
        }
        public async Task<List<Posts>> GetPostForForumWithID(int id)
        {
            Log.Debug("Getting Posts from the database by forumID: {0}", id);
            return await _context.Posts.Select(post => post)
                .Where(post => post.ForumID == id)
                .ToListAsync();
        }
        public async Task<List<Posts>> GetAllPostsAsync()
        {
            try
            {
                Log.Debug("Getting all Posts from the database.");
                return await _context.Posts.AsNoTracking()
                .Select(
                    posts => posts
                ).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error("Failed to retrieve all Posts \n" + e.Message);
                return null;
            }
        }
        public async Task<Posts> GetPostsAsync(Posts posts)
        {

            Posts found = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(post => post.PostID == posts.PostID);
            if (found == null)
            {
                Log.Error("Post does not exist!");
                return null;
            }
            Log.Debug("Getting Posts from the database: {0}", posts.PostID);
            return new Posts(found.PostID, found.Topic, found.UserName, found.ForumID);
        }

        //Done with Posts & starting with Comments

        public async Task<Comments> AddCommentsAsync(Comments comments)
        {
            await _context.Comments.AddAsync(
                comments
                );
            await _context.SaveChangesAsync();
            Log.Debug("Adding Comment into the database: {0}", comments.PostID);
            return comments;
        }
        public async Task<int> DeleteCommentsAsync(int commentID)
        {
            Comments toBeDeleted = _context.Comments.AsNoTracking().First(comm => comm.CommentID == commentID);
            _context.Comments.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            Log.Debug("Removing Comment from the database: {0}", commentID);
            return commentID;
        }
        public async Task<Comments> UpdateCommentsAsync(Comments comments)
        {
            _context.Comments.Update(comments);
            await _context.SaveChangesAsync();
            Log.Debug("Updating Comment from the database: {0}", comments.CommentID);
            return comments;
        }
        public async Task<List<Comments>> GetCommentsByIdAsync(int id)
        {
            Log.Debug("Getting Comment from the database by ID: {0}", id);
            return await _context.Comments.Select(comm => comm)
                .Where(comm => comm.PostID == id)
                .ToListAsync();
        }
        public async Task<List<Comments>> GetAllCommentsAsync()
        {
            Log.Debug("Getting all Comments from the database");
            return await _context.Comments.AsNoTracking()
            .Select(
                comments => comments
            ).ToListAsync();
        }

        public async Task<Comments> GetCommentsAsync(Comments comments)
        {
            Comments found = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(comm => comm.PostID == comments.PostID);
            if (found == null) return null;
            Log.Debug("Getting Comment from the database: {0}", comments.PostID);
            return new Comments(found.CommentID, found.PostID, found.UserName, found.Created, found.Message);
        }

        public async Task<Posts> GetPostByPostID(int id)
        {
            return await _context.Posts.FindAsync(id);
        }
    }
}
