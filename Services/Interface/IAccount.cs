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
        public string? Auth(string login,  string password);
        public List<Account> Search(string? firstName, string? lastName, string? email, int from = 0, int size = 10);
        public Account UpdateAccount(int id, Account account);
    }
}
