namespace HWWebApi.Models
{
    public class Memory : IModel<Memory>
    {
        public long Id { get; set; }
        public long Capacity { get; set; }
        public RamType Type { get; set; } 
        public long Ghz { get; set; }

        public bool EqualByMembers(Memory memory)
        {
            return Capacity == memory.Capacity &&
                   Type == memory.Type &&
                   Ghz == memory.Ghz;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ Capacity.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ Ghz.GetHashCode();
                return hashCode;
            }
        }
    }
}