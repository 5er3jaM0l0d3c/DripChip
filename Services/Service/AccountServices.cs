
using Entities;

using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Interface.HighLevel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AccountServices
    {
        private readonly IAccount account;

        public AccountServices(IAccount account)
        {
            this.account = account;
        }

        public Account Registration(Account account)
        {
            return this.account.Add(account);
        }

        public Account? GetAccount(int id)
        {
            return account.Get(id);
        }

        public List<Account> SearchAccount(Dictionary<string, object> filters)
        {
            return this.account.Search(filters);
        }

        public Account UpdateAccount(int id, Account account)
        {
            return this.account.Update(id, account);
        }

        public void DeleteAccount(int id)
        {
            this.account.Delete(id);
        }
    }
}
