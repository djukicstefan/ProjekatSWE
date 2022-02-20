using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<List<Comment>> GetAllCommentsFromUser(User user);
        Task<Comment> GetCommentById(int id);
        Task<Comment> CreateNewAsync(Comment order);
        Task RemoveComment(int id);
    }
}
