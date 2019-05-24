namespace Models
{
    public class Processor : IModel<Processor>
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public long? GHz { get; set; } 
        public int? NumOfCores { get; set; }
        public Architecture? Architecture { get; set; } 

        public bool EqualByMembers(Processor processor)
        {
            return string.Equals(Name, processor.Name) &&
                   GHz == processor.GHz &&
                   NumOfCores == processor.NumOfCores &&
                   Architecture == processor.Architecture;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ GHz.GetHashCode();
                hashCode = (hashCode * 397) ^ NumOfCores.GetHashCode();
                hashCode = (hashCode * 397) ^ Architecture.GetHashCode();
                return hashCode;
            }
        }
    }
}