using System;
using System.Collections.Generic;
using System.Text;
using HW.Bot.Interfaces;

namespace HW.Bot.Model
{
    internal class PersonalData : IPersonalData
    {
        public string Name { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string WorkArea { get; set; }
    }
}
