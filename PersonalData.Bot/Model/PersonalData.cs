using System;
using System.Collections.Generic;
using System.Text;
using PersonalData.Bot.Interfaces;

namespace PersonalData.Bot.Model
{
    internal class PersonalData : IPersonalData
    {
        public string Name { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string Work { get; set; }
    }
}
