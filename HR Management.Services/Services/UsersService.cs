using HR_Management.data.Repositories;
using HR_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Management.data;
using System.Dynamic;
using System.Collections;

namespace HR_Management.Services.Services;


public class UsersService : IUserService
{

    private readonly IUserRepository _userRepository;
    private DateTime _data;

    public DateTime HireDate { get; set; }
    public DateTime LastSalary { get; set; }


    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> DeleteUser(int id)
    {
        return _userRepository.DeleteUser(id);
    }



    public async Task<int> GetCountManager()
    {
        if (Convert.ToInt32(await _userRepository.GetCountManager()) >= 2)
        {
            throw new Exception("Maximum number of users reached, there are only 2 manager");
        }

        var count = await _userRepository.GetCountManager();
        return count;
    }

    public async Task<int> GetCountSpecialist()
    {

        if (Convert.ToInt32(await _userRepository.GetCountSpecialist()) >= 4)
        {
            throw new Exception("Maximum number of users reached, there are only 4 specialist");
        }

        var count = await _userRepository.GetCountSpecialist();
        return count;
    }

    public async Task<int> GetCountWorker()
    {
        if (Convert.ToInt32(await _userRepository.GetCountWorker()) >= 4)
        {
            throw new Exception("Maximum number of users reached, there are only 4 workers");
        }

        var count = await _userRepository.GetCountWorker();
        return count;
    }




    public async Task<User> GetDetails(int id)
    {
        return _userRepository.GetDetails(id);
    }

    public async Task<bool> InsertUser(User user)
    {
        if (user.Rol == "Specialist" && Convert.ToInt32(await _userRepository.GetCountSpecialist()) >= 4)
        {
            throw new Exception("Maximum number of users reached, only 4 specialists are allowed");
        }
        if (user.Rol == "Worker" && Convert.ToInt32(await _userRepository.GetCountWorker()) >= 4)
        {
            throw new Exception("Maximum number of users reached, only 4 workers are allowed");
        }
        if (user.Rol == "Manager" && Convert.ToInt32(await _userRepository.GetCountManager()) >= 2)
        {
            throw new Exception("Maximum number of users reached, only 2 managers are allowed");
        }
        if (Convert.ToInt32(await _userRepository.GetCountAll()) >= 10)
        {
            throw new Exception("Maximum number of users are 10");
        }



        return await _userRepository.InsertUser(user);
    }

    public async Task<bool> UpdateUser(User user)
    {



        if (user.Rol == "Specialist" && Convert.ToInt32(await _userRepository.GetCountSpecialist()) >= 4)
        {
            throw new Exception("Maximum number of users reached, only 4 specialists are allowed");
        }
        if (user.Rol == "Worker" && Convert.ToInt32(await _userRepository.GetCountWorker()) >= 4)
        {
            throw new Exception("Maximum number of users reached, only 4 workers are allowed");
        }
        if (user.Rol == "Manager" && Convert.ToInt32(await _userRepository.GetCountManager()) >= 2)
        {
            throw new Exception("Maximum number of users reached, only 2 managers are allowed");
        }
        if (Convert.ToInt32(await _userRepository.GetCountAll()) >= 10)
        {
            throw new Exception("Maximum number of users are 10");
        }



        return await _userRepository.UpdateUser(user);
    }

    public async Task<bool> ValidateSalaryAndDate(int id)
    {
        var user = _userRepository.GetSalaryAndStartDateByID(id);

        DateTime currentDate = DateTime.Now;
        float salarySuperior = 0;
        DateTime InitialDate = user.WorkingStartDate;

        if (InitialDate > currentDate)
        {
            throw new Exception("The date can not be superior than the working start date");
        }

        TimeSpan workingDuration = currentDate - user.WorkingStartDate;
        int monthsSinceLastRevision = (int)(workingDuration.TotalDays / 30.44);

        if (monthsSinceLastRevision >= 3)
        {
            salarySuperior = CalculateSalaryIncrement(user.Salary, user.Rol);
            if (monthsSinceLastRevision < 3)
            {
                return await _userRepository.UpdateSalaryById(id, user.Salary);
            }

            float newSalary = user.Salary + salarySuperior;

            user.Salary = newSalary;
            return await _userRepository.UpdateSalaryById(id, newSalary);
        }
        return false;
    }

    public void ValidateUser(User user)
    {

        if (string.IsNullOrEmpty(user.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrEmpty(user.LastName))
        {
            throw new ArgumentException("Last name is required.");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException("Email is required.");
        }
        if (user.PersonalAddress == null)
        {
            throw new ArgumentException("Personal address is required.");
        }
        if (string.IsNullOrEmpty(user.WorkingStartDate.ToString()))
        {
            throw new ArgumentException("Working start date is required.");
        }
        if (string.IsNullOrEmpty(user.Rol))
        {
            throw new ArgumentException("Rol is required.");
        }
        if (string.IsNullOrEmpty(user.Salary.ToString()))
        {
            throw new ArgumentException("Salary is required.");
        }
        if (user.Rol != "Manager" && user.Rol != "Specialist" && user.Rol != "Worker")
        {
            throw new ArgumentException("This role does not exist in the HR Program");


        }

    }

    public Task<bool> UpdateSalaryByid(int id, float salary)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAllUsers()
    {

        return _userRepository.GetAllUsers();

    }

    public async Task ExtractDataWithSalaryIncrement()
    {
        var users = await GetAllUsers();

        var dynamicList = users.Select(x =>
        {
            dynamic expando = new ExpandoObject();
            expando.Value = x;
            return expando;
        }).ToList();


        var userCsvList = dynamicList.Select(u => {
            dynamic expando = new ExpandoObject();

            // Add properties from the original object to the ExpandoObject
            var dictionary = (IDictionary<string, object>)expando;
            dictionary["Id"] = u.Value.Id;
            dictionary["Name"] = u.Value.Name;
            dictionary["LastName"] = u.Value.LastName;
            dictionary["Email"] = u.Value.Email;
            dictionary["PersonalAddress"] = u.Value.PersonalAddress;
            dictionary["Phone"] = u.Value.Phone;
            dictionary["WorkingStartDate"] = u.Value.WorkingStartDate;
            dictionary["Rol"] = u.Value.Rol;
            dictionary["Salary"] = u.Value.Salary;

            return expando;
        }).ToList();

        for (int i = 0; i < userCsvList.Count; i++)
        {
            var user = userCsvList[i];
            TimeSpan workingDuration = DateTime.Now - user.WorkingStartDate;
            int monthsSinceLastRevision = (int)(workingDuration.TotalDays / 30.44);

            if (monthsSinceLastRevision >= 3)
            {
                float missingPeriods = monthsSinceLastRevision / 3;
                var actualPeriod = 1;
                var actualSalary = user.Salary;
                while(actualPeriod < missingPeriods)
                {
                    actualSalary = CalculateSalaryIncrement(actualSalary, user.Rol);
                    IDictionary<string, object> dictionary = user;
                    dictionary.Add($"NewPeriodSalary{actualPeriod}", actualSalary);
                    user.NewPeriodSalary = actualSalary;
                    actualPeriod++;
                }

            }

            var builder = new StringBuilder();
            builder.AppendLine("Id,Name,LastName,Email,PersonalAddress,Phone,WorkingStartDate,Rol,Salary, NewPeriodSalary1, NewPeriodSalary2, NewPeriodSalary3, NewPeriodSalary4, NewPeriodSalary5, NewPeriodSalary6,NewPeriodSalary7,NewPeriodSalary8");

            foreach (var item in userCsvList)
            {
                List<string> nombres = new List<string> { $"{item.Id},{item.Name},{item.LastName},{item.Email},{item.PersonalAddress},{item.Phone},{item.WorkingStartDate},{item.Rol},{item.Salary}" };
                var salariesList = new List<string>();
                IDictionary<string, object> dictionary = item;
                var filteredProperties = dictionary.Where(x => x.Key.StartsWith("NewPeriodSalary"));
                foreach (var property in filteredProperties)
                {
                    string propertyValue = property.Value.ToString();
                    nombres.Add(propertyValue);
                }
                builder.AppendLine(string.Join(", ", nombres));
            }

            var fileName = "users.csv";
            var filePath = @"C:\Users\Carlo\Documents\" + fileName;

            await System.IO.File.WriteAllTextAsync(filePath, builder.ToString());
        }
    }

    private float CalculateSalaryIncrement(float salary, string rol)
    {
        float salarySuperior = 0;
        switch (rol)
        {
            case "Manager":
                salarySuperior = (float)(salary * 0.012);
                break;

            case "Worker":
                salarySuperior = (float)(salary * 0.05);
                break;

            case "Specialist":
                salarySuperior = (float)(salary * 0.08);
                break;

            default:
                return 0;
        }

        return salarySuperior;
        
    }
}


