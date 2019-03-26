﻿using PersonalData.Bot.Model;

namespace PersonalData.Bot.Interfaces
{
    public interface IPersonalData
    {
        string Name { get; set; }
        /// <summary>
        /// 0= NOT_DEFINE, 1= MALE, 2= FEMALE
        /// </summary>
        int Gender { get; set; }
        int Age { get; set; }
        string WorkArea { get; set; }
    }
}