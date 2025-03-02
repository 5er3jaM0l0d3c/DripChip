using Entities;
using Microsoft.IdentityModel.Tokens;
using Services.Interface.HighLevel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Service.Mid
{
    public class AccountDB : IAccount
    {
        private DripChipContext context { get; set; }
        public AccountDB(DripChipContext context)
        {
            this.context = context;
        }

        public Account Add(Account entity)
        {
            if (context.Account.FirstOrDefault(x => x.Email == entity.Email) != null)
            {
                throw new Exception("409");
            }

            context.Account.Add(entity);
            context.SaveChanges();

            Account acc = context.Account.FirstOrDefault(x => x.Email == entity.Email && x.Password == entity.Password);

            acc.Password = null;
            return acc;

        }

        public Account? Get(long id)
        {
            var account = context.Account.FirstOrDefault(x => x.Id == id);

            return account;
        }

        public string? Auth(string login, string password)
        {
            var acc = context.Account.FirstOrDefault(x => x.Email == login && x.Password == password);

            if (acc != null)
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

        public List<Account> Search(Dictionary<string, object> filters)
        {
            List<Account> accounts = context.Account.ToList();

            if (filters.ContainsKey("FirstName"))
            {
                accounts = accounts.Intersect(accounts.Where(x => x.FirstName.Contains(filters["FirstName"].ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            if (filters.ContainsKey("LastName"))
            {
                accounts = accounts.Intersect(accounts.Where(x => x.LastName.Contains(filters["LastName"].ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            if (filters.ContainsKey("Email"))
            {
                accounts = accounts.Intersect(accounts.Where(x => x.Email.Contains(filters["Email"].ToString(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            try
            {
                if (filters.ContainsKey("From"))
                {
                    accounts.RemoveRange(0, (int)filters["From"]);
                }
            }
            catch
            {
                throw new IndexOutOfRangeException("From is out of range");
            }

            try
            {

                if (filters.ContainsKey("Size"))
                {
                    accounts = accounts.GetRange(0, (int)filters["Size"]);
                }
                else
                {
                    accounts = accounts.GetRange(0, 10);
                }
            }
            catch { }

            return accounts;
        }

        public Account Update(long id, Account entity)
        {
            entity.Id = (int)id;

            var acc = context.Account.FirstOrDefault(x => x.Id == id);

            if (acc == null)
            {
                throw new Exception("403");
            }

            if (acc.Email != entity.Email && entity.Email != null && context.Account.FirstOrDefault(x => x.Email == entity.Email) != null)
            {
                throw new Exception("409");
            }

            context.Entry(acc).CurrentValues.SetValues(entity);
            context.SaveChanges();

            entity.Password = null;

            return entity;
        }

        public void Delete(long id)
        {
            var account = context.Account.FirstOrDefault(x => x.Id == id);

            if (account == null)
            {
                throw new Exception("403");
            }

            if (context.Animal.FirstOrDefault(x => x.ChipperId == id) != null)
            {
                throw new Exception("400");
            }

            context.Account.Remove(account);
            context.SaveChanges();
        }
    }
}
