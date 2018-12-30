using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class Disk
    {
        public long Id { get; set; }
        public DiskType? Type { get; set; }
        public int? Rpm { get; set; } 
        public long? Capacity { get; set; } 

        public override bool Equals(object obj)
        {
            return obj is Disk disk &&
                   Type == disk.Type &&
                   Rpm == disk.Rpm &&
                   Capacity == disk.Capacity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Rpm, Capacity);
        }
    }
}