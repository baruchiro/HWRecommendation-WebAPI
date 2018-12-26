using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class GPU
    {
        public long Id { get; set; }
        public int cores { get; set; }

        public override bool Equals(object obj)
        {
            var he = obj as GPU;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.cores.Equals(he.cores);
        }
    }
}