using System;

namespace HWWebApi.Models
{
    public class MotherBoard : IModel<MotherBoard>
    {
        public long Id { get; set; }
        public int? DdrSockets { get; set; }
        public long? MaxRam { get; set; }
        public int? SataConnections { get; set; }
        public Architecture? Architecture { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }

        public bool EqualByMembers(MotherBoard board)
        {
            return DdrSockets == board.DdrSockets &&
                   MaxRam == board.MaxRam &&
                   SataConnections == board.SataConnections &&
                   Architecture == board.Architecture &&
                   string.Equals(Manufacturer, board.Manufacturer, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(Product, board.Product, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (DdrSockets?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MaxRam?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (SataConnections?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Architecture?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Manufacturer?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Product?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}