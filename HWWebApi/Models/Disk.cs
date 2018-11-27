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
    }
}