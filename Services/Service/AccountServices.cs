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
                var claims = new List<Claim> { new Claim("Name", login), new Claim("Password", password) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthenticationOptions.ISSUER,
                        audience: AuthenticationOptions.AUDIENCE,
                claims: claims,
                signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            return null;
        }
    }
}
