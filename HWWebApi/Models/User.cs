using System;
using System.Collections.Generic;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using HWWebApi.Helpers;
using Models;
using Models.ModelEqualityComparer;

namespace HWWebApi.Models
{
    public class User : Person, IPersonalData
    {
        public ICollection<UserChannel> Channels { get; set; }
        public ICollection<Scan> Scans { get; set; }

        public bool EqualByMembers(User user)
        {
            return user != null &&
                   EqualByMembers(user) &&
                   Channels.IsEquals(user.Channels, new ModelEqualityByMembers<UserChannel>()) &&
                   Scans.IsEquals(user.Scans);
        }

        public override int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = base.GetHashCodeWithMembers();
                hashCode = (hashCode * 397) ^ (Channels?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Scans?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
