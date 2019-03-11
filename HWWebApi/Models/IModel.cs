using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HWWebApi.Models
{
    public interface IModel<T> where T:IModel<T>
    {
        bool EqualByMembers(T model);
        int GetHashCodeWithMembers();
        long Id { get; set; }
    }
}
