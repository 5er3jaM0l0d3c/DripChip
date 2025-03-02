using Entities;
using Services.Interface.LowLevel;
using Services.Interface.MidLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.HighLevel
{
    public interface IAccount : ICRUD<Account>, ISearchable<Account>, ISignIn<Account>
    {
    }
}
