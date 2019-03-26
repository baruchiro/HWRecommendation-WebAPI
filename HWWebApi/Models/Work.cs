using System.ComponentModel.DataAnnotations;

namespace HWWebApi.Models
{
    public class Work : IModel<Work>
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }

        public bool EqualByMembers(Work model)
        {
            return model.Name.Equals(Name);
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                return hashCode;
            }
        }
    }
}