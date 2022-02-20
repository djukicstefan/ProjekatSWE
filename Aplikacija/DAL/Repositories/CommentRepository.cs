using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext ctx) : base(ctx) { }
    }
}
