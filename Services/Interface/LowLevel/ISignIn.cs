using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.LowLevel
{
    public interface ISignIn<T>
    {   
        public string? Auth(string login, string password);
    }
}
