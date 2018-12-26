using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class Disk
    {
        public long Id { get; set; }
        public DiskType type { get; set; }
        public int rpm { get; set; }
        public long capacity { get; set; }

        public override bool Equals(object obj)
        {
            var he = obj as Disk;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.type.Equals(he.type) &&
                this.rpm.Equals(he.rpm) &&
                this.capacity.Equals(he.capacity);
        }
    }
}