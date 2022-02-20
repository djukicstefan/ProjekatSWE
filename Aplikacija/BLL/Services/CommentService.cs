using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : Service, ICommentService
    {
        public CommentService(IUnitOfWork uow) : base(uow) { }

        public async Task<Comment> CreateNewAsync(Comment comment)
        {
            var c = await Comments.AddAsync(comment);
            comment.CreatedAt = DateTime.Now;
            await SaveChangesAsync();
            return c;
        }

        public Task<List<Comment>> GetAllCommentsAsync()
            => Comments.GetAllAsync("User");

        public async Task<List<Comment>> GetAllCommentsFromUser(User user)
            => (await GetAllCommentsAsync()).Where(x => x.User.Id == user.Id).ToList();

        public Task<Comment> GetCommentById(int id)
            => Comments.GetByIdAsync(id);

        public async Task RemoveComment(int id)
        {
            await Comments.DeleteAsync(id);
            await SaveChangesAsync();
        }
    }
}
