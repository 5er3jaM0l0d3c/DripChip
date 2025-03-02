using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.LowLevel
{
    public interface IUpdateable<T>
    {
        public T Update(long id, T entity);
    }
}
