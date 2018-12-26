namespace HWWebApi.Models
{
    public class Memory
    {
        public long Id { get; set; }
        public int Capacity { get; set; }
        public RAMType type { get; set; }
        public long ghz { get; set; }

        public override bool Equals(object obj)
        {
            var he = obj as Memory;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.Capacity.Equals(he.Capacity) &&
                this.type.Equals(he.type) &&
                this.ghz.Equals(he.ghz);
        }
    }
}