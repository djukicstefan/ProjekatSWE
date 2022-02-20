using DAL.Interfaces;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Menu : IEntity
    {
        public int Id { get; set; }
        public DaysInWeek Day { get; set; }
        public virtual ICollection<Food> Foods { get; set; }

        public Menu()
        {
            Foods = new HashSet<Food>();
        }

        public enum DaysInWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
    }
}
