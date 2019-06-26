using HW.Bot.Interfaces;

namespace HW.Bot.Model
{
    internal class PersonalData : IPersonalData
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string WorkArea { get; set; }
    }
}
