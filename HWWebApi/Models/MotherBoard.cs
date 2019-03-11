using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class MotherBoard : IModel<MotherBoard>
    {
        public long Id { get; set; }
        public int? DdrSockets { get; set; }
        public long? MaxRam { get; set; } 
        public int? SataConnections { get; set; } 
        public Architecture? Architecture { get; set; }

        public bool EqualByMembers(MotherBoard board)
        {
            return DdrSockets == board.DdrSockets &&
                   MaxRam == board.MaxRam &&
                   SataConnections == board.SataConnections &&
                   Architecture == board.Architecture;
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ DdrSockets.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxRam.GetHashCode();
                hashCode = (hashCode * 397) ^ SataConnections.GetHashCode();
                hashCode = (hashCode * 397) ^ Architecture.GetHashCode();
                return hashCode;
            }
        }
    }
}