using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace HR_Management.Model
    {
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PersonalAddress { get; set; }
            public int? Phone { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "YYYY-MM-DD HH:MM:SS")]
            public DateTime WorkingStartDate { get; set; } 
            
            public byte[] Picture { get; set; }
            public string Rol { get; set; }
            public float Salary { get; set; } = 0.0f;
        public User(int id,String name,String lastName,String email,String personalAddress,int phone, DateTime workingStartDate, byte[] picture, string role, float salary)
            {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PersonalAddress = personalAddress;
            Phone = phone;
            WorkingStartDate = workingStartDate;
            Picture = picture;
            Rol = role;
            Salary = salary;
        }
        public User()
        {
           
        }
       
    }
    }

