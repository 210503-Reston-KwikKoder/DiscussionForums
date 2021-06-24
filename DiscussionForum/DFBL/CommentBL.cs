using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;
using DFDL;

namespace DFBL
{
    class CommentBL :IComment
    {
        private readonly IRepo _repo;
        public CommentBL(DFDBContext context)
        {
            _repo = new Repo(context);
        }
        public async Task<List<Comments>> GetAllComments()
        {
            return await _repo.GetAllCommentsAsync();
        }

        public async Task<List<Comments>> GetComment(int commentID)
        {
            return await _repo.GetCommentsByIdAsync(commentID);
        }

        public async Task<Comments> AddComment(Comments comment)
        {
            return await _repo.AddCommentsAsync(comment);
        }

        public async Task<Comments> RemoveComments(Comments comments)
        {

            return await _repo.DeleteCommentsAsync(comments);

        }

        public async Task<Comments> UpdateComment(Comments comments)
        {
            return await _repo.UpdateCommentsAsync(comments);
        }
    }
}
