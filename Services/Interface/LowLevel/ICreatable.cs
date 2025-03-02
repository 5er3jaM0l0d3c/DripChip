using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.LowLevel
{
    public interface ICreatable<T>
    {
        public T Add(T entity);
    }
}
