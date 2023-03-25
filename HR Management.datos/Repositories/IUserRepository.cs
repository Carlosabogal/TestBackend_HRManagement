using HR_Management.data;
using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.data.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        User GetDetails(int id);
        User GetSalaryAndStartDateByID(int id);

        Task<bool> InsertUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<int> GetCountWorker();
        Task<int> GetCountSpecialist();
        Task<int> GetCountManager();
        Task<int> GetCountAll();

        Task<bool> UpdateSalaryById(int id, float salary);


    }
}
