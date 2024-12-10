using Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AccountServices : IAccount
    {
        private DripChipContext context { get; set; }
        public AccountServices(DripChipContext context)
        {
            this.context = context;
        }

        public Account? Registration(Account account)
        { 
            context.Account.Add(account);
            context.SaveChanges();

            Account? acc = context.Account.FirstOrDefault(x => x.Email == account.Email && x.Password == account.Password);

            if (acc != null)
            {
                acc.Password = null;
                return acc;
            }
            return null;
        }

        public Account? GetAccountInfo(int id)
        {
            var account = context.Account.FirstOrDefault(x => x.Id == id);
            if (account != null)
            {
                account.Password = null;
                return account;
            }
            return null;
        }
    }
}
