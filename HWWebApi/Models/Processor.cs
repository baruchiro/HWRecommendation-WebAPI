namespace HWWebApi.Models
{
    public class Processor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ghz { get; set; }
        public int numOfCores { get; set; }
        public Architacture architacture { get; set; }
    }
}