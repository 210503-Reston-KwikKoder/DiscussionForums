﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;
using DFDL;

namespace DFBL
{
    public class ForumBL :IForum
    {
        private readonly IRepo _repo;
        public ForumBL(IRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Forum>> GetAllForums()
        {
            return await _repo.GetAllForumsAsync();
        }

        public async Task<Forum> GetForum(int forumID)
        {
            return await _repo.GetForumByIdAsync(forumID);
        }

        public async Task<Forum> AddForum(Forum forum)
        {
            return await _repo.AddForumAsync(forum);
        }

        public async Task<int> RemoveForum(int forumID)
        {
            
            return await _repo.DeleteForumAsync(forumID);
     
        }

        public async Task<Forum> UpdateForum(Forum forum)
        {
            return await _repo.UpdateForumAsync(forum);
        }
    }
}
