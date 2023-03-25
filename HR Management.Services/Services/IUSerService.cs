using HR_Management.Model;
using HR_Management.data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Services.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetDetails(int id);
        Task<bool> InsertUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<int> GetCountWorker();
        Task<int> GetCountSpecialist();
        Task<int> GetCountManager();
        public void ValidateUser(User user);
        public Task<bool> ValidateSalaryAndDate(int id);
        Task ExtractDataWithSalaryIncrement();
    }

}

