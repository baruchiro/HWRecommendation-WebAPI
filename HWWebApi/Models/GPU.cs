namespace HWWebApi.Models
{
    public class GPU : IModel<GPU>
    {
        public long Id { get; set; }
        public int? Cores { get; set; }

        public bool EqualByMembers(GPU other)
        {
            return Cores == other.Cores;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                return 397 ^ Cores.GetHashCode();
            }
        }
    }
}