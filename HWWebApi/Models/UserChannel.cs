using System;
using Models;

namespace HWWebApi.Models
{
    public class UserChannel : IModel<UserChannel>
    {
        public long Id { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }

        public bool EqualByMembers(UserChannel model)
        {
            return ChannelId.Equals(model.ChannelId, StringComparison.CurrentCultureIgnoreCase) &&
                   UserId.Equals(model.UserId, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (ChannelId?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (UserId?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
