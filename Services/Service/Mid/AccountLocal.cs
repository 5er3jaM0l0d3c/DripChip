using Entities;
using Services.Interface.HighLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.Mid
{
    public class AccountLocal : IAccount
    {
        public Account Add(Account entity)
        {
            throw new NotImplementedException();
        }

        public string? Auth(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Account? Get(long id)
        {
            throw new NotImplementedException();
        }

        public List<Account> Search(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public Account Update(long id, Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
