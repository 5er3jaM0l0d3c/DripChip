using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.LowLevel
{
    public interface ISearchable<T>
    {
        public List<T> Search(Dictionary<string, object> filters);
    }
}