namespace HWWebApi.Models
{
    public class Processor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ghz { get; set; }
        public int numOfCores { get; set; }
        public Architacture architacture { get; set; }

        public override bool Equals(object obj)
        {
            var he = obj as Processor;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.Name.Equals(he.Name) &&
                this.ghz.Equals(he.ghz) &&
                this.numOfCores.Equals(he.numOfCores) &&
                this.architacture.Equals(he.architacture);
        }
    }
}