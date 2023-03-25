using Dapper;
using HR_Management.Model;
using MySql.Data.MySqlClient;
using Mysqlx.Datatypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HR_Management.data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString;
      
        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteUser(int idUser)
        {
            var db = dbConnection();
            var sql = @"DELETE FROM users WHERE id = @id";
            var result = await db.ExecuteAsync(sql, new { id = idUser });
            return result > 0;
        }

      
        public User GetDetails(int idUser)
        {
            var user=new User();
            var db = dbConnection();
            db.Open();
            var sql = @"SELECT id, name, last_name, email, personal_address, phone, DATE_FORMAT(working_start_date, '%Y-%m-%d %H:%i:%s') as working_start_date , picture,rol, salary FROM users where id = @id";
            using var command = new MySqlCommand(sql, db);
            command.Parameters.AddWithValue("@id", 1);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("name");
                var lastName = reader.GetString("last_name");
                var email = reader.GetString("email");
                var personalAddress = reader.GetString("personal_address");
                var phone = reader.GetInt32("phone");
                var workingStartDate = reader.GetDateTime("working_start_date");
                var picture = reader.IsDBNull(reader.GetOrdinal("picture")) ? null : (byte[])reader["picture"];
                var rol = reader.GetString("rol");
                var salary = reader.GetFloat("salary");
                 user = new User(id, name, lastName, email, personalAddress, phone, workingStartDate, picture, rol, salary);

            }
            return user;
        }

        public async Task<bool> InsertUser(User user)
        {
            var db = dbConnection();
            var initSalary = user.Salary;
            var sql = @"INSERT INTO users (name, last_name, email, personal_address, phone, working_start_date, picture,rol, initial_salary, salary) 
                VALUES (@name, @lastName, @email, @personalAddress, @phone, @workingStartDate, @picture,@rol,@initSalary, @salary)";

            var result = await db.ExecuteAsync(sql,new
            { user.Name, user.LastName, user.Email, user.PersonalAddress, user.Phone,user.WorkingStartDate,user.Picture,user.Rol, initSalary , user.Salary });



            return result > 0;
        }

      

        public async Task<int> GetCountWorker()
        {
            var db = dbConnection();
            var countQuery = "SELECT COUNT(*) FROM Users WHERE Rol='Worker'";
            var count = await db.ExecuteScalarAsync<int>(countQuery);

            return count;
        }

        public async Task<int> GetCountSpecialist()
        {
            var db = dbConnection();
            var countQuery = "SELECT COUNT(*) FROM Users WHERE Rol='Specialist'";
            var count = await db.ExecuteScalarAsync<int>(countQuery);

            return count;
        }
        public async Task<int> GetCountManager()
        {
            var db = dbConnection();
            var countQuery = "SELECT COUNT(*) FROM Users WHERE Rol='Manager'";
            var count = await db.ExecuteScalarAsync<int>(countQuery);

            return count;
        }

        public async Task<int> GetCountAll()
        {
            var db = dbConnection();
            var countQuery = "SELECT COUNT(*) FROM Users";
            var count = await db.ExecuteScalarAsync<int>(countQuery);
            return count;
        }      

        public async Task<bool> UpdateUser(User user)
        {
            var db = dbConnection();
            var sql = @"UPDATE users SET name = @name,    last_name = @lastName,    email = @email,     personal_address = @personalAddress,     phone = @phone,    working_start_date = @workingStartDate,   picture = @picture,   rol = @rol,   salary = @salary WHERE id = @id;";

            var result = await db.ExecuteAsync(sql, new
            { user.Name, user.LastName, user.Email, user.PersonalAddress, user.Phone, user.WorkingStartDate, user.Picture, user.Rol, user.Salary, user.Id });



            return result > 0;
        }

 

        public async Task<bool> UpdateSalaryById(int id, float salary)
        {
            var db = dbConnection();
            var sql = @"UPDATE users SET salary = @salary  where id = @id;";
            var result = await db.ExecuteAsync(sql, new { id = id, salary = salary });
            return result > 0;
        }

        public  Task<User> GetDateByID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT WorkingStartDate FROM Users WHERE Id = @userId;";
            return db.QueryFirstOrDefaultAsync<User>(sql, new { id = id });

        }

        public async Task<List<User>> GetAllUsers()
        {
            var userList = new List<User>();
            var db = dbConnection();
            await db.OpenAsync();
            var sql = @"SELECT * FROM users";
            using var command = new MySqlCommand(sql, db);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("name");
                var lastName = reader.GetString("last_name");
                var email = reader.GetString("email");
                var personalAddress = reader.GetString("personal_address");
                var phone = reader.GetInt32("phone");
                var workingStartDate = reader.GetDateTime("working_start_date");
                var picture = reader.IsDBNull(reader.GetOrdinal("picture")) ? null : (byte[])reader["picture"];
                var rol = reader.GetString("rol");
                var salary = reader.GetFloat("salary");
                var user = new User(id, name, lastName, email, personalAddress, phone, workingStartDate, picture, rol, salary);
                userList.Add(user);
            }
            return userList;
        }

        public User GetSalaryAndStartDateByID(int id)
        {
            var user = new User();
            var db = dbConnection();
            db.Open();
            var sql = @"SELECT DATE_FORMAT(working_start_date, '%Y-%m-%d %H:%i:%s') as working_start_date, salary, rol FROM users where id = @id";
            using var command = new MySqlCommand(sql, db);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var workingStartDate = reader.GetDateTime("working_start_date");
                var salary = reader.GetFloat("salary");
                var rol = reader.GetString("rol");
                user = new User(id, "", "", "", "", 0, workingStartDate, null, rol, salary);
            }
            return user;
        }

    }
}
