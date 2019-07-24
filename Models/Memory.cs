using System;

namespace Models
{
    public class Memory : IModel<Memory>
    {
        public long Id { get; set; }
        public long Capacity { get; set; }
        public RamType Type { get; set; }
        public long Ghz { get; set; }
        public string BankLabel { get; set; }
        public string DeviceLocator { get; set; }
        public int Generation { get; set; }

        public bool EqualByMembers(Memory memory)
        {
            return memory != null &&
                   Capacity == memory.Capacity &&
                   Type == memory.Type &&
                   Ghz == memory.Ghz &&
                   string.Equals(BankLabel, memory.BankLabel, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(DeviceLocator, memory.DeviceLocator, StringComparison.CurrentCultureIgnoreCase) &&
                   Generation == memory.Generation;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ Capacity.GetHashCode();
                hashCode = (hashCode * 397) ^ ((int)Type).GetHashCode();
                hashCode = (hashCode * 397) ^ Ghz.GetHashCode();
                hashCode = (hashCode * 397) ^ (BankLabel?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DeviceLocator?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Generation.GetHashCode();
                return hashCode;
            }
        }
    }
}