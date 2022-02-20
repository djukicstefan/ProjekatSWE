namespace DAL.Interfaces
{
    public interface IEntity : IEntity<int> {}

    public interface IEntity<K>
    {
        public K Id { get; set; }
    }
}
