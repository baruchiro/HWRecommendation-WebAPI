using Models.ModelEqualityComparer;

namespace Models
{
    public class Gpu : IModel<Gpu>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Processor { get; set; }
        public int? Cores { get; set; }
        public string Manufacturer { get; set; }
        public Memory Memory { get; set; }
        public int Version { get; set; }

        public bool EqualByMembers(Gpu other)
        {
            return other != null &&
                   Name == other.Name &&
                   Processor == other.Processor &&
                   Cores == other.Cores &&
                   Manufacturer == other.Manufacturer &&
                   ModelEqualityByMembers<Memory>.EqualByMembers(Memory, other.Memory) &&
                   Version == other.Version;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Processor?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Cores.GetHashCode();
                hashCode = (hashCode * 397) ^ (Manufacturer?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Memory?.GetHashCodeWithMembers() ?? 0);
                hashCode = (hashCode * 397) ^ Version.GetHashCode();
                return hashCode;
            }
        }
    }
}