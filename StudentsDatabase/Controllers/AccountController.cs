using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentsDatabase.Helper;
using StudentsDatabase.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace StudentsDatabase.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public AccountController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Register (RegisterStudents vm)
        {
            string UserExistsQuery = $"Select * from [StudentsDb] where Username='{vm.Username}'"+ $" OR Email='{vm.Email}'";
            bool userExists = _helper.UserAlreadyExists(UserExistsQuery);

            if (userExists == true)
            {
                ViewBag.Error = "Username and Email Already Exists";
                return View("Register", "Accounts");
            }

            string Query = "Insert into [StudentsDb] (Username, Email, Password, Name, Contact)values('{vm.Username}', '{vm.Email}', '{vm.Password}', '{vm.Name}' ,'{vm.Contact}')";

            int result = _helper.DMLTransaction(Query);
            if (result > 0)
            {
                EntryIntoSession(vm.Username);
                ViewBag.Success = "Successfully registered.";
                return View();
            }
            return View();

        }


        private void EntryIntoSession(string Username)
        {
            HttpContext.Session.SetString("Username", Username);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
