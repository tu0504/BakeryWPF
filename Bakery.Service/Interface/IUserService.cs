using Bakery.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Service.Interface
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        void SaveUser(User user);
        void DeleteCustomer(User user);
        void UpdateCustomer(User user);
        List<User> SearchByNameOrEmail(string searchTerm);

    }
}
