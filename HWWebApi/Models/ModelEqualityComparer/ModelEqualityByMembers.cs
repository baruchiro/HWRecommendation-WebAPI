using System.Collections.Generic;

namespace HWWebApi.Models.ModelEqualityComparer
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
    }
}
