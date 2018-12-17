namespace HWWebApi.Models
{
    public class Memory
    {
        public long Id { get; set; }
        public int Capacity { get; set; }
        public RAMType type { get; set; }
        public long ghz { get; set; }
    }
}