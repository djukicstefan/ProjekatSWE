using DAL.Interfaces;
using System;

namespace DAL.Models
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
