using System.Collections.Generic;

namespace Models.ModelEqualityComparer
{
    public class ModelEqualityComparer<T> : IEqualityComparer<IModel<T>>
    where T : IModel<T>
    {
        public bool Equals(IModel<T> x, IModel<T> y)
        {
            return y is T t &&
                   x.Id == t.Id &&
                   x.EqualByMembers(t);
        }

        public int GetHashCode(IModel<T> obj)
        {
            return (obj.GetHashCodeWithMembers() * 397) ^ obj.GetHashCode(); 
        }
    }
}
