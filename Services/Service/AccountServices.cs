using DripChip;
using Entities;

using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
            if (context.Account.FirstOrDefault(x => x.Email == account.Email) != null)
            {
                throw new Exception("409");
            }

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

            return account;
        }

        public string? Auth(string login, string password)
        {
            var acc = context.Account.FirstOrDefault(x => x.Email == login && x.Password == password);
            
            if(acc != null)
            {
                var claims = new List<Claim> { new Claim("Name", login), new Claim("Password", password), new Claim("Id", acc.Id.ToString()) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthenticationOptions.ISSUER,
                        audience: AuthenticationOptions.AUDIENCE,
                claims: claims,
                signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            return null;
        }

        public List<Account> Search(string? firstName, string? lastName, string? email, int from = 0, int size = 10)
        {
            List<Account> accounts = context.Account.ToList();

            if (firstName != null)
            {
                accounts = accounts.Intersect(SearchByFirstName(firstName)).ToList();
            }

            if (lastName != null)
            {
                accounts = accounts.Intersect(SearchByLastName(lastName)).ToList();
            }

            if (email != null)
            {
                accounts = accounts.Intersect(SearchByEmail(email)).ToList();
            }

            try
            {
                accounts.RemoveRange(0, from);
                accounts.RemoveRange(size, accounts.Count - size);
            }
            catch { }

            return accounts;
        }

        private List<Account> SearchByFirstName(string firstName)
        {
            return context.Account.Where(x => x.FirstName.ToLower().Contains(firstName.ToLower())).ToList();
        }

        private List<Account> SearchByLastName(string lastName)
        {
            return context.Account.Where(x => x.LastName.ToLower().Contains(lastName.ToLower())).ToList();
        }

        private List<Account> SearchByEmail(string email) 
        {
            return context.Account.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
        }

        public Account UpdateAccount(int id, Account account)
        {
            account.Id = id;

            var acc = context.Account.FirstOrDefault(x => x.Id == id);

            if (acc == null)
                throw new Exception("403");
            
            if (acc.Email != account.Email && account.Email != null && context.Account.FirstOrDefault(x => x.Email == account.Email) != null)
                throw new Exception("409");

            context.Entry(acc).CurrentValues.SetValues(account);
            context.SaveChanges();

            account.Password = null;

            return account;
        }
    }
}
