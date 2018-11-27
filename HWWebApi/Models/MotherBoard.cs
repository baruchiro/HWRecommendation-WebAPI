using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class MotherBoard
    {
        public long Id { get; set; }
        public int ddrSockets { get; set; }
        public long maxRam { get; set; }
        public int sataConnections { get; set; }
        public Architacture architacture { get; set; }
    }
}