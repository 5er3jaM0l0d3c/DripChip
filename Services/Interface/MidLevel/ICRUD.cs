using Services.Interface.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.MidLevel
{
    public interface ICRUD<T> : IGettable<T>, IUpdateable<T>, ICreatable<T>, IDeleteable<T>
    {
    }
}
