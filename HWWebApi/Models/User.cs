using System;
using System.Collections.Generic;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using HWWebApi.Helpers;
using HWWebApi.Models.ModelEqualityComparer;

namespace HWWebApi.Models
{
    public class User : IModel<User>, IPersonalData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string WorkArea { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public ICollection<UserChannel> Channels { get; set; }

        public bool EqualByMembers(User model)
        {
            return Name == model.Name &&
                   WorkArea.Equals(model.WorkArea, StringComparison.CurrentCultureIgnoreCase)&&
                   Age == model.Age &&
                   Gender == model.Gender &&
                   Channels.IsEquals(model.Channels, new ModelEqualityByMembers<UserChannel>());
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (WorkArea?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Age.GetHashCode();
                hashCode = (hashCode * 397) ^ Gender.GetHashCode();
                hashCode = (hashCode * 397) ^ (Channels?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
