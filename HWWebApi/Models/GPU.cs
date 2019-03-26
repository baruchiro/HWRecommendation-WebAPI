using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class GPU : IModel<GPU>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Processor { get; set; }
        public int? Cores { get; set; }

        public bool EqualByMembers(GPU other)
        {
            return Name == other.Name &&
                   Processor == other.Processor &&
                   Cores == other.Cores;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Processor.GetHashCode();
                hashCode = (hashCode * 397) ^ Cores.GetHashCode();
                return hashCode;
            }
        }
    }
}