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

        public override bool Equals(object obj)
        {
            var he = obj as MotherBoard;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.ddrSockets.Equals(he.ddrSockets) &&
                this.maxRam.Equals(he.maxRam) &&
                this.sataConnections.Equals(he.sataConnections) &&
                this.architacture.Equals(he.architacture);
        }
    }
}