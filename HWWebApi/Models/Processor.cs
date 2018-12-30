using System;

namespace HWWebApi.Models
{
    public class Processor
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public long? GHz { get; set; } 
        public int? NumOfCores { get; set; }
        public Architecture? Architecture { get; set; } 

        public override bool Equals(object obj)
        {
            return obj is Processor processor &&
                   Name == processor.Name &&
                   GHz == processor.GHz &&
                   NumOfCores == processor.NumOfCores &&
                   Architecture == processor.Architecture;
        }

        public override int GetHashCode() => HashCode.Combine(Name, GHz, NumOfCores, Architecture);
    }
}