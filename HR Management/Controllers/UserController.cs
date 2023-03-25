using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HR_Management.data.Repositories;
using HR_Management.Model;
using HR_Management.Services.Services;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;
using System.Text;

namespace HR_Management.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {

            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> ExportAllUsers()
        {
            await _userService.ExtractDataWithSalaryIncrement();

            return Ok();
        }


        [HttpPut("/{id}/salary")]
        public async Task<ActionResult<bool>> UpdateSalaryByid(int id)
        {

            return await _userService.ValidateSalaryAndDate(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {

            return Ok(await _userService.GetDetails(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.DeleteUser(id)) {
                return Ok("The user was deleted correctly");
            }

            return Ok("The user to delete does not exist");
        }
    

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {


            var id = Convert.ToInt32(Request.Form["Id"]);
            var name = Request.Form["Name"];
            var lastName = Request.Form["LastName"];
            var email = Request.Form["Email"];
            var personalAddress = Request.Form["PersonalAddress"];
            var phone =Convert.ToInt32(Request.Form["Phone"]);
            var workingStartDate = DateTime.Parse(Request.Form["WorkingStartDate"]);
            var picture = Request.Form.Files["Picture"];
            var role = Request.Form["Role"];
            var salary = Request.Form["Salary"];
            float payment = 0;

            if (string.IsNullOrEmpty(salary))
            {
                throw new Exception("The salary is required.");

            }
            else
            {

                payment = float.Parse(salary);
            }


            if (picture == null)
            {
                throw new Exception("The picture is required.");
            }
            byte[] pictureData;

            using (var ms = new MemoryStream())
            {
                picture.CopyTo(ms);
                pictureData = ms.ToArray();
            }
         
            User user = new User(id,name,lastName, email, personalAddress, phone, workingStartDate, pictureData, role, payment);
            _userService.ValidateUser(user);
            if (user == null)
            {
                return BadRequest();
            }
            if (user == null)
                return BadRequest();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var created= await _userService.InsertUser(user);
            return Created("created",user);



        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id)
        {

            
            var name = Request.Form["Name"];
            var lastName = Request.Form["LastName"];
            var email = Request.Form["Email"];
            var personalAddress = Request.Form["PersonalAddress"];
            var phone = Convert.ToInt32(Request.Form["Phone"]);
            var workingStartDate = DateTime.Parse(Request.Form["WorkingStartDate"]);
            var picture = Request.Form.Files["Picture"];
            var role = Request.Form["Role"];
            var salary = Request.Form["Salary"];
            float payment = 0;

            if (string.IsNullOrEmpty(salary))
            {
                throw new Exception("The salary is required.");

            }
            else
            {

                payment = float.Parse(salary);
            }


            if (picture == null)
            {
                throw new Exception("The picture is required.");
            }
            byte[] pictureData;

            using (var ms = new MemoryStream())
            {
                picture.CopyTo(ms);
                pictureData = ms.ToArray();
            }

            User user = new User(id, name, lastName, email, personalAddress, phone, workingStartDate, pictureData, role, payment);
            _userService.ValidateUser(user);
            if (user == null)
            {
                return BadRequest();
            }
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _userService.UpdateUser(user);
            return Created("created", user);

        }



    }
}

