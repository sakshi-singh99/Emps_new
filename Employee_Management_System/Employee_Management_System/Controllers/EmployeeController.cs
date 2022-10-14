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
                _context.Employees.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Registered Successfully!";
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
                var objCatlist = (from e in _context.Employees
                              join m in _context.Roles on e.RoleId equals m.RoleId
                              where e.RoleId==m.RoleId select new
                              {
                                  Id = e.EmpId,
                                  Firstname = e.EmpFirstName,
                                  Lastname = e.EmpLastName,
                                  Email = e.EmpEmail,
                                  Password = e.EmpPassword,
                                  Address = e.EmpAddress,
                                  Phone = e.EmpPhoneNumber,
                                  Roleid = e.RoleId,
                                  Rolename = m.RoleName,
                                  Date = e.EmpRegisteredDate,
                              }).ToList();
                var get_user = objCatlist.Single(p => p.Email == empobj.EmpEmail
                               && p.Password == empobj.EmpPassword);
                if (get_user != null)
                {
                    HttpContext.Session.SetString("EmpId", get_user.Id.ToString());
                    HttpContext.Session.SetString("FirstName", get_user.Firstname.ToString());
                    HttpContext.Session.SetString("LastName", get_user.Lastname.ToString());
                    HttpContext.Session.SetString("Email", get_user.Email.ToString());
                    HttpContext.Session.SetString("Address", get_user.Address.ToString());
                    HttpContext.Session.SetString("Phone", get_user.Phone.ToString());
                    HttpContext.Session.SetString("RoleId", get_user.Roleid.ToString());
                    HttpContext.Session.SetString("RoleName", get_user.Rolename.ToString());
                    HttpContext.Session.SetString("Date", get_user.Date.ToString());
                    string role = HttpContext.Session.GetString("RoleId");
                    if (role == "1")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }  
                    if (role=="2")
                    {
                        return RedirectToAction("Dashboard", "Project");
                    } 
                    if (role=="3")
                    {
                        return RedirectToAction("Dashboard", "Employee");
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

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Employee");
            }
        }
        public IActionResult GetProject()
        {
            //List<Map> objCatlist = new List<Map>();
            //objCatlist = (from e in _context.Employees
            //              join m in _context.Maps on e.EmpId equals m.EmpId
            //              select m).ToList();
            //int id = Convert.ToInt32(HttpContext.Session.GetInt32("EmpId"));
            //if (objCatlist.Any(p=>p.EmpId==id))
            //{
            //    return View(objCatlist);
            //}

            List<Employee> employee = _context.Employees.ToList();
            List<Project> project = _context.Projects.ToList();
            List<Map> map = _context.Maps.ToList();
            var getProject = from m in map
                             join e in employee on m.EmpId equals e.EmpId into table1
                             from e in table1.ToList()
                             join p in project on m.ProjectId equals p.ProjectId into table2
                             from p in table2.ToList()
                             select new UiModel
                             {
                                 Mapui = m,
                                 EmployeeUi = e,
                                 ProjectUi = p
                             };

            //var objgetProject = (from g in getProject
            //                     join m in _context.Maps on g.EmployeeUi.EmpId equals m.EmpId
            //                     where g.EmployeeUi.EmpId == m.EmpId
            //                     select new
            //                     {
            //                       Id = m.ProjectId,
            //                       Name  = g.ProjectUi.ProjectName
            //                     }).ToList();

            //var get_project = getProject.Single(p => p.EmployeeUi.EmpId == p.Mapui.EmpId
            //                 && p.ProjectUi.ProjectId == p.Mapui.ProjectId);

            string email = HttpContext.Session.GetString("Email");
            if (getProject.Any(p =>p.EmployeeUi.EmpEmail==email))
            {
                return View (getProject);
            }

            return View();
        }
    }
}
