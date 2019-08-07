using HWWebApi.Helpers;
using Models;
using Models.ModelEqualityComparer;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class User : Person
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
