using System.Collections.Generic;

namespace Models.ModelEqualityComparer
{
    public class ModelEqualityByMembers<T> : IEqualityComparer<IModel<T>>
        where T : IModel<T>
    {
        public bool Equals(IModel<T> x, IModel<T> y)
        {
            return y is T t &&
                   x.EqualByMembers(t);
        }

        public int GetHashCode(IModel<T> obj)
        {
            return obj.GetHashCodeWithMembers();
        }

        public static bool EqualByMembers(T x, T y)
        {
            return x?.EqualByMembers(y) ?? y == null;
        }
    }
}
