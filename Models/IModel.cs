namespace Models
{
    public interface IModel<in T> where T : IModel<T>
    {
        bool EqualByMembers(T model);
        int GetHashCodeWithMembers();
        long Id { get; set; }
    }
}
