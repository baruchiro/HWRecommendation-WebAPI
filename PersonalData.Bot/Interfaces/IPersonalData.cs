using HW.Bot.Model;

namespace HW.Bot.Interfaces
{
    public interface IPersonalData
    {
        string Name { get; set; }
        /// <summary>
        /// 0= NOT_DEFINE, 1= MALE, 2= FEMALE
        /// </summary>
        Gender Gender { get; set; }
        int Age { get; set; }
        string WorkArea { get; set; }
    }
}