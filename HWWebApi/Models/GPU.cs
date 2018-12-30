using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class GPU
    {
        public long Id { get; set; }
        public int? Cores { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GPU gpu &&
                   Cores == gpu.Cores;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cores);
        }
    }
}