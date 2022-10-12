using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Net;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            List<Role> RoleDD = new List<Role>();
            RoleDD = (from c in _context.Roles select c).ToList();
            RoleDD.Insert(0, new Role { RoleId = 0, RoleName = "---Select Role---" });
            ViewBag.message = RoleDD;
            return View();

        }

        [HttpPost]
        public IActionResult Registration(Employee empobj)
        {
          
            var get_user = _context.Employees.FirstOrDefault(email => email.EmpEmail == empobj.EmpEmail);
            if(get_user == null)
            {
              //  string query = $"spRegister {empobj.EmpFirstName}," +
              //$"{empobj.EmpLastName},{empobj.EmpEmail},{empobj.EmpPassword},{empobj.EmpAddress},{empobj.EmpPhoneNumber}" +
              //$"{empobj.RoleId},{empobj.EmpRegisteredDate}";
              //  var empRecord = _context.Employees.FromSqlRaw(query).ToList();
                _context.Employees.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Resistration Successfully!";
                return RedirectToAction("Login");
            }
            else {
                TempData["ResultOk"] = "Employee allready exist!";

            }

            return View(empobj);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee empobj)
        {
            try
            {
                var get_user = _context.Employees.Single(p => p.EmpEmail == empobj.EmpEmail
                               && p.EmpPassword == empobj.EmpPassword);
                var role = _context.Roles.Any(r=>r.RoleId==1);
                if (get_user != null)
                {
                    HttpContext.Session.SetString("Email", get_user.EmpEmail.ToString());
                    HttpContext.Session.SetString("Password", get_user.EmpPassword.ToString());
                    HttpContext.Session.SetString("FirstName", get_user.EmpFirstName.ToString());
                    HttpContext.Session.SetString("LastName", get_user.EmpLastName.ToString());
                    HttpContext.Session.SetString("Address", get_user.EmpAddress.ToString());
                    HttpContext.Session.SetString("Phone", get_user.EmpPhoneNumber.ToString());
                    HttpContext.Session.SetString("RoleId", get_user.RoleId.ToString());
                    HttpContext.Session.SetString("Date", get_user.EmpRegisteredDate.ToString());
                    if (role == true)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["LoginResult"] = "Email or Password does not match";
                }
            }
            catch
            {
                TempData["LoginResult"] = "Email or Password does not match";

            }

            return View();
        }

        public ActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }

        }

      
          
     
    }
}
