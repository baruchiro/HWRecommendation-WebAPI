using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class MotherBoard
    {
        public long Id { get; set; }
        public int? DdrSockets { get; set; }
        public long? MaxRam { get; set; } 
        public int? SataConnections { get; set; } 
        public Architecture? Architecture { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MotherBoard board &&
                   DdrSockets == board.DdrSockets &&
                   MaxRam == board.MaxRam &&
                   SataConnections == board.SataConnections &&
                   Architecture == board.Architecture;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DdrSockets, MaxRam, SataConnections, Architecture);
        }
    }
}