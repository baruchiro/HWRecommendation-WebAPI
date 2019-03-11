using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class Disk : IModel<Disk>
    {
        public long Id { get; set; }
        public DiskType? Type { get; set; }
        public int? Rpm { get; set; } 
        public long? Capacity { get; set; }

        public bool EqualByMembers(Disk disk)
        {
            return Type == disk.Type &&
                   Rpm == disk.Rpm &&
                   Capacity == disk.Capacity;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Rpm.GetHashCode();
                hashCode = (hashCode * 397) ^ Capacity.GetHashCode();
                return hashCode;
            }
        }
    }
}