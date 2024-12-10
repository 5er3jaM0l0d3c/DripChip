using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAccount
    {
        public Account? Registration(Account account);
        public Account? GetAccountInfo(int id);
    }
}
